using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// IHazTest
/// </summary>
public interface IHazTest : ICompile, IHazContent
{
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
        var testProjects = Solution.GetProjects(testProjectName);

        foreach (var testProject in testProjects)
        {
            var testResultsDirectory = GetContentDirectory(testProject);

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
                        .EnableNoBuild()
                        .SetCustomDotNetTestSettings(customDotNetTestSettings)
                        .When(testResults, _ => _
                            .SetLoggers("trx")
                            .SetResultsDirectory(testResultsDirectory)));
                }
                catch (Exception ex)
                {
                    Serilog.Log.Logger.Error($"Exception: {ex}");
                    if (testBuildStopWhenFailed) throw;
                }

                if (testResults)
                    testFailed |= ReportTestCount(testProject, configuration);
            }
        }

        if (testFailed & testBuildStopWhenFailed)
        {
            throw new Exception($"Test Failed = {testFailed}");
        }
    }

    /// <summary>
    /// ReportTestCount
    /// </summary>
    /// <param name="testProject"></param>
    /// <param name="configuration"></param>
    /// <returns>Return if fail some test</returns>
    bool ReportTestCount(Project testProject, string configuration = "")
    {
        var testResultsDirectory = GetContentDirectory(testProject);

        IEnumerable<string> GetOutcomes(AbsolutePath file)
            => XmlTasks.XmlPeek(
                file,
                "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));

        var resultFiles = Globbing.GlobFiles(testResultsDirectory, "*.trx");
        var outcomes = resultFiles.SelectMany(GetOutcomes).ToList();
        var passedTests = outcomes.Count(x => x == "Passed");
        var failedTests = outcomes.Count(x => x == "Failed");
        var skippedTests = outcomes.Count(x => x == "NotExecuted");

        var message = $"ReportTest: {testProject.Name} ({configuration}) \t Passed: {passedTests} \t Skipped: {skippedTests} \t Failed: {failedTests}";
        Serilog.Log.Logger.Information(message);

        if (skippedTests > 0)
            Serilog.Log.Warning(message);

        if (failedTests > 0)
            Serilog.Log.Error(message);

        if (passedTests == 0 && skippedTests == 0)
            return true;

        return failedTests > 0;
    }
}