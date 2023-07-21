using Newtonsoft.Json.Bson;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazTest
    /// </summary>
    public interface IHazTest : ICompile, IHazContent, IHazGitHubActions
    {
        /// <summary>
        /// GetTestDirectory
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public AbsolutePath GetTestDirectory(Project project) => project.Directory / "bin" / "Tests";

        /// <summary>
        /// TestProjects
        /// </summary>
        /// <param name="testProjectName"></param>
        /// <param name="testResults"></param>
        /// <param name="testBuildStopWhenFailed"></param>
        /// <param name="customDotNetTestSettings"></param>
        /// <exception cref="Exception"></exception>
        public void TestProjects(string testProjectName,
            bool testResults = true,
            bool testBuildStopWhenFailed = true,
            Func<DotNetTestSettings, DotNetTestSettings> customDotNetTestSettings = null)
        {
            var testFailed = false;
            var testProjects = Solution.GetAllProjects(testProjectName);

            foreach (var testProject in testProjects)
            {
                var testResultsDirectory = GetTestDirectory(testProject);

                var configurations = testProject.GetReleases();
                foreach (var configuration in configurations)
                {
                    try
                    {
                        Serilog.Log.Logger.Information($"Build: {testProject.Name} {configuration}");
                        testProject.Build(configuration);
                        Serilog.Log.Logger.Information($"Test: {testProject.Name} {configuration}");

                        DotNetTasks.DotNetTest(_ => _
                            .SetProjectFile(testProject)
                            .SetConfiguration(configuration)
                            .SetVerbosity(DotNetVerbosity.Normal)
                            .EnableNoBuild()
                            .SetCustomDotNetTestSettings(customDotNetTestSettings)
                            .When(testResults, _ => _
                                .SetLoggers($"trx;LogFileName={ProjectTestFileName(testProject, configuration)}")
                                .SetResultsDirectory(testResultsDirectory)));
                    }
                    catch (Exception ex)
                    {
                        Serilog.Log.Logger.Information($"Exception: {ex}");
                        testFailed = true;
                    }

                    if (testResults)
                        testFailed |= ReportTestProject(testProject, configuration);
                }
            }

            if (testResults)
            {
                ReportTestProjects(testProjects);
            }

            if (testFailed & testBuildStopWhenFailed)
            {
                throw new Exception($"Test Failed = {testFailed}");
            }
        }

        /// <summary>
        /// ReportTestProjects
        /// </summary>
        /// <param name="testProjects"></param>
        void ReportTestProjects(IEnumerable<Project> testProjects)
        {
            var testFiles = testProjects.SelectMany(testProject =>
                    Globbing.GlobFiles(GetTestDirectory(testProject), ProjectTestFileName(testProject, "*"))
                );

            var testReport = TrxExtension.GetTestReport(testFiles);
            var passedTests = testReport.Passed;
            var failedTests = testReport.Failed;
            var skippedTests = testReport.Skipped;

            ReportSummary(_ => _
                .When(failedTests > 0, _ => _
                    .AddPair("Failed", failedTests.ToString()))
                .AddPair("Passed", passedTests.ToString())
                .When(skippedTests > 0, _ => _
                    .AddPair("Skipped", skippedTests.ToString())));

            foreach (var testFile in testFiles)
            {
                Serilog.Log.Logger.Information($"TestFile: {testFile}");
            }

            try
            {
                ReportTestProjectsGitHubSummary(testFiles);
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Warning($"ReportTestProjectsGitHubSummary Fail");
                Serilog.Log.Logger.Information($"ReportTestProjectsGitHubSummary {ex}");
            }
        }

        /// <summary>
        /// Simple ReportTestProjectsGitHubSummary
        /// </summary>
        /// <param name="resultFiles"></param>
        void ReportTestProjectsGitHubSummary(IEnumerable<AbsolutePath> resultFiles)
        {
            if (resultFiles.IsEmpty()) return;

            var summaryTestReport = TestResultUtil.Markdown.GetSummaryTestReports(resultFiles);
            GitHubSummary(summaryTestReport);

            foreach (var resultFile in resultFiles)
            {
                var testReport = TestResultUtil.Markdown.GetDetailsTestReport(resultFile);
                GitHubSummary(testReport);
            }
        }

        /// <summary>
        /// ProjectTestFileName
        /// </summary>
        /// <param name="testProject"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        string ProjectTestFileName(Project testProject, string configuration)
        {
            return $"{testProject.Name}_{configuration}.trx";
        }

        /// <summary>
        /// ReportTestProject
        /// </summary>
        /// <param name="testProject"></param>
        /// <param name="configuration"></param>
        /// <returns>Return if fail some test</returns>
        bool ReportTestProject(Project testProject, string configuration)
        {
            var testResultsDirectory = GetTestDirectory(testProject);

            var resultFiles = Globbing.GlobFiles(testResultsDirectory, ProjectTestFileName(testProject, configuration));
            var outcomes = resultFiles.SelectMany(GetTestFileOutcomes).ToList();
            var passedTests = outcomes.Count(x => x == "Passed");
            var failedTests = outcomes.Count(x => x == "Failed");
            var skippedTests = outcomes.Count(x => x == "NotExecuted");

            var message = $"ReportTest: {testProject.Name} ({configuration}) \t Passed: {passedTests} \t Skipped: {skippedTests} \t Failed: {failedTests}";
            Serilog.Log.Logger.Information(message);

            if (failedTests > 0)
                Serilog.Log.Error(message);

            else if (skippedTests > 0)
                Serilog.Log.Warning(message);

            if (passedTests == 0 && skippedTests == 0)
                return true;

            return failedTests > 0;
        }

        #region GetTestFile
        /// <summary>
        /// GetTestFileOutcomes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IEnumerable<string> GetTestFileOutcomes(AbsolutePath file)
            => XmlTasks.XmlPeek(
                file,
                "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));
        /// <summary>
        /// GetTestFileDurations
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        IEnumerable<TimeSpan> GetTestFileDurations(AbsolutePath file)
            => XmlTasks.XmlPeek(
                file,
                "/xn:TestRun/xn:Results/xn:UnitTestResult/@duration",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")).Select(TimeSpan.Parse);
        #endregion
    }
}