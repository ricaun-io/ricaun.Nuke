using Nuke.Common.IO;
using Nuke.Common.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ricaun.Nuke.Utils
{
    /// <summary>
    /// TestRunUtil
    /// </summary>
    public class TestRunUtil
    {
        /// <summary>
        /// OUTCOME_PASSED
        /// </summary>
        public const string TEST_OUTCOME_PASSED = "Passed";
        /// <summary>
        /// OUTCOME_FAILED
        /// </summary>
        public const string TEST_OUTCOME_FAILED = "Failed";
        /// <summary>
        /// OUTCOME_SKIPPED ("NotExecuted")
        /// </summary>
        public const string TEST_OUTCOME_SKIPPED = "NotExecuted";

        #region Model
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static class Model
        {
            [XmlRoot("UnitTestResult", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
            public class UnitTestResult
            {
                [XmlAttribute("executionId")]
                public Guid ExecutionId { get; set; }

                [XmlAttribute("testId")]
                public Guid TestId { get; set; }

                [XmlAttribute("testName")]
                public string TestName { get; set; }

                [XmlAttribute("computerName")]
                public string ComputerName { get; set; }

                [XmlAttribute("duration")]
                public string Duration { get; set; }

                [XmlAttribute("startTime")]
                public DateTime StartTime { get; set; }

                [XmlAttribute("endTime")]
                public DateTime EndTime { get; set; }

                [XmlAttribute("testType")]
                public Guid TestType { get; set; }

                [XmlAttribute("outcome")]
                public string Outcome { get; set; }

                [XmlAttribute("testListId")]
                public Guid TestListId { get; set; }

                [XmlAttribute("relativeResultsDirectory")]
                public string RelativeResultsDirectory { get; set; }

                [XmlElement("Output")]
                public Output Output { get; set; }
            }
            public class Output
            {
                [XmlElement("StdOut")]
                public string StdOut { get; set; }

                [XmlElement("StdErr")]
                public string StdErr { get; set; }

                [XmlElement("ErrorInfo")]
                public ErrorInfo ErrorInfo { get; set; }
            }
            public class ErrorInfo
            {
                [XmlElement("Message")]
                public string Message { get; set; }

                [XmlElement("StackTrace")]
                public string StackTrace { get; set; }
            }

            [XmlRoot(ElementName = "TestRun", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
            public class TestRun
            {
                [XmlElement("Times", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
                public Times Times { get; set; }
                [XmlElement("Results", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
                public Results Results { get; set; }
                [XmlElement("TestDefinitions", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
                public TestDefinitions TestDefinitions { get; set; }
            }

            [XmlRoot("Times")]
            public class Times
            {
                [XmlAttribute("creation")]
                public DateTime Creation { get; set; }
                [XmlAttribute("queuing")]
                public DateTime Queuing { get; set; }
                [XmlAttribute("start")]
                public DateTime Start { get; set; }
                [XmlAttribute("finish")]
                public DateTime Finish { get; set; }
            }

            [XmlRoot("Results", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
            public class Results
            {
                [XmlElement("UnitTestResult", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
                public List<UnitTestResult> UnitTestResult { get; set; }
            }

            [XmlRoot("TestDefinitions")]
            public class TestDefinitions
            {
                [XmlElement("UnitTest")]
                public List<UnitTest> UnitTests { get; set; }

                [XmlRoot("UnitTest")]
                public class UnitTest
                {
                    [XmlElement("Execution")]
                    public Execution Execution { get; set; }
                    [XmlElement("TestMethod")]
                    public TestMethod TestMethod { get; set; }
                    [XmlAttribute("name")]
                    public string Name { get; set; }
                    [XmlAttribute("storage")]
                    public string Storage { get; set; }
                    [XmlAttribute("id")]
                    public Guid Id { get; set; }
                }

                [XmlRoot("Execution")]
                public class Execution
                {
                    [XmlAttribute("id")]
                    public Guid Id { get; set; }
                }

                [XmlRoot("TestMethod")]
                public class TestMethod
                {
                    [XmlAttribute("codeBase")]
                    public string CodeBase { get; set; }
                    [XmlAttribute("adapterTypeName")]
                    public string AdapterTypeName { get; set; }
                    [XmlAttribute("className")]
                    public string ClassName { get; set; }
                    [XmlAttribute("name")]
                    public string Name { get; set; }
                }
            }
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        #endregion

        /// <summary>
        /// Markdown
        /// </summary>
        public class Markdown
        {
            /// <summary>
            /// GetSummaryTestReports
            /// </summary>
            /// <param name="resultFiles"></param>
            /// <returns></returns>
            public static string GetSummaryTestReports(IEnumerable<AbsolutePath> resultFiles)
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("|     | Test File | Passed | Failed | Skipped | Total | Time |");
                stringBuilder.AppendLine("| :-: | --------- | :----: | :----: | :-----: | :---: | :--: |");

                foreach (var resultFile in resultFiles)
                {
                    var testReport = TestReportUtils.GetTestReport(resultFile);
                    var fileName = resultFile.Name;
                    var passedTests = testReport.Passed;
                    var failedTests = testReport.Failed;
                    var skippedTests = testReport.Skipped;
                    var totalSeconds = testReport.TotalSeconds;

                    var totalTests = passedTests + failedTests + skippedTests;

                    var resultIcon =
                        (failedTests != 0) ? IconFailed :
                        (skippedTests != 0) ? IconSkipped :
                        IconPassed;

                    stringBuilder.AppendLine($"| {resultIcon} | {fileName} | {passedTests} | {failedTests} | {skippedTests} | {totalTests} | {totalSeconds:0.00} |");
                }

                stringBuilder.AppendLine();

                return stringBuilder.ToString();
            }

            /// <summary>
            /// GetDetailsTestReport
            /// </summary>
            /// <param name="resultFile"></param>
            /// <returns></returns>
            public static string GetDetailsTestReport(AbsolutePath resultFile)
            {
                var testRun = GetTestRun(resultFile);

                var testResults = testRun.Results.UnitTestResult
                    .OrderBy(e => GetTestMethodClassName(testRun, e.TestId) + e.TestName);

                var fileIcon = GetIcon(testResults.Select(r => r.Outcome).ToArray());
                var fileName = resultFile.Name;

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("<details>");
                stringBuilder.AppendLine("<summary>");
                stringBuilder.AppendLine($"<strong>{fileIcon} {fileName}</strong>");
                stringBuilder.AppendLine("</summary>");
                stringBuilder.AppendLine("<br>");
                stringBuilder.AppendLine();

                stringBuilder.AppendLine("|     | Test Class | Test Name | Time | Message | Error |");
                stringBuilder.AppendLine("| :-: | ---------- | --------- | :--: | ------- | ----- |");

                foreach (var testResult in testResults)
                {
                    var icon = GetIcon(testResult.Outcome);
                    var testClass = GetTestMethodClassName(testRun, testResult.TestId);
                    var testName = testResult.TestName;
                    var totalSeconds = testResult.Duration is null ? 0 : TimeSpan.Parse(testResult.Duration).TotalSeconds;
                    var message = GetMessage(testResult);
                    var error = GetError(testResult);
                    stringBuilder.AppendLine($"| {icon} | {testClass} | {testName} | {totalSeconds:0.00} | {Pre(message)} | {Pre(error)} |");
                }

                stringBuilder.AppendLine();
                stringBuilder.AppendLine("</details>");
                stringBuilder.AppendLine();

                return stringBuilder.ToString();
            }

            #region Utils
            const string IconFailed = ":red_circle:";
            const string IconSkipped = ":yellow_circle:";
            const string IconPassed = ":green_circle:";
            /// <summary>
            /// GetIcon
            /// </summary>
            /// <param name="outcomes"></param>
            /// <returns></returns>
            public static string GetIcon(params string[] outcomes)
            {
                var icon =
                    outcomes.Any(e => e.Contains(TEST_OUTCOME_FAILED)) ? IconFailed :
                    outcomes.Any(e => e.Contains(TEST_OUTCOME_SKIPPED)) ? IconSkipped :
                    IconPassed;
                return icon;
            }

            /// <summary>
            /// GetTestMethodClassName
            /// </summary>
            /// <param name="testRun"></param>
            /// <param name="id"></param>
            /// <returns></returns>
            public static string GetTestMethodClassName(Model.TestRun testRun, Guid id)
            {
                var testMethod = testRun.TestDefinitions.UnitTests.FirstOrDefault(e => e.Id == id);
                if (testMethod != null)
                {
                    return testMethod.TestMethod.ClassName;
                }
                return string.Empty;
            }

            /// <summary>
            /// Pre
            /// </summary>
            /// <returns></returns>
            public static string Pre(string content)
            {
                if (string.IsNullOrEmpty(content))
                    return string.Empty;
                return $"<pre>{content}</pre>";
            }

            /// <summary>
            /// GetMessage
            /// </summary>
            /// <param name="testResult"></param>
            /// <returns></returns>
            public static string GetMessage(Model.UnitTestResult testResult)
            {
                var message = $"{testResult.Output?.StdOut}\n{testResult.Output?.StdErr}";
                message = message.Trim();
                return message.Replace("\r", "").Replace("\n", "<br>");
            }

            /// <summary>
            /// GetError
            /// </summary>
            /// <param name="testResult"></param>
            /// <returns></returns>
            public static string GetError(Model.UnitTestResult testResult)
            {
                var message = $"{testResult.Output?.ErrorInfo?.Message}\n{testResult.Output?.ErrorInfo?.StackTrace}";
                message = message.Trim();
                return message.Replace("\r", "").Replace("\n", "<br>");
            }
            #endregion
        }

        /// <summary>
        /// GetTestRun
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static Model.TestRun GetTestRun(AbsolutePath file)
        {
            return file.ReadXml<Model.TestRun>();
        }
    }
}