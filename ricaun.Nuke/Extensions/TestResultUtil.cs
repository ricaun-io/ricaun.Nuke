using Nuke.Common.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// TestResultUtil
    /// </summary>
    public class TestResultUtil
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

        /// <summary>
        /// UnitTestResult
        /// </summary>
        [XmlRoot("UnitTestResult", Namespace = "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")]
        public class UnitTestResult
        {
            //[XmlAttribute("executionId")]
            //public Guid ExecutionId { get; set; }

            //[XmlAttribute("testId")]
            //public Guid TestId { get; set; }
            /// <summary>
            /// TestName
            /// </summary>

            [XmlAttribute("testName")]
            public string TestName { get; set; }
            /// <summary>
            /// ComputerName
            /// </summary>

            [XmlAttribute("computerName")]
            public string ComputerName { get; set; }
            /// <summary>
            /// Duration
            /// </summary>

            [XmlAttribute("duration")]
            public string Duration { get; set; }
            /// <summary>
            /// StartTime
            /// </summary>

            [XmlAttribute("startTime")]
            public DateTime StartTime { get; set; }
            /// <summary>
            /// EndTime
            /// </summary>

            [XmlAttribute("endTime")]
            public DateTime EndTime { get; set; }
            /// <summary>
            /// TestType
            /// </summary>

            [XmlAttribute("testType")]
            public Guid TestType { get; set; }
            /// <summary>
            /// Outcome
            /// </summary>

            [XmlAttribute("outcome")]
            public string Outcome { get; set; }

            //[XmlAttribute("testListId")]
            //public Guid TestListId { get; set; }

            //[XmlAttribute("relativeResultsDirectory")]
            //public string RelativeResultsDirectory { get; set; }
            /// <summary>
            /// Output
            /// </summary>

            [XmlElement("Output")]
            public Output Output { get; set; }
        }
        /// <summary>
        /// Output
        /// </summary>
        public class Output
        {
            /// <summary>
            /// StdOut
            /// </summary>
            [XmlElement("StdOut")]
            public string StdOut { get; set; }

            /// <summary>
            /// StdErr
            /// </summary>
            [XmlElement("StdErr")]
            public string StdErr { get; set; }

            /// <summary>
            /// ErrorInfo
            /// </summary>

            [XmlElement("ErrorInfo")]
            public ErrorInfo ErrorInfo { get; set; }
        }
        /// <summary>
        /// ErrorInfo
        /// </summary>
        public class ErrorInfo
        {
            /// <summary>
            /// Message
            /// </summary>
            [XmlElement("Message")]
            public string Message { get; set; }

            /// <summary>
            /// StackTrace
            /// </summary>
            [XmlElement("StackTrace")]
            public string StackTrace { get; set; }
        }
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
                    var testReport = TrxExtension.GetTestReport(resultFile);
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
                var testResults = GetTestFileResults(resultFile);
                var fileIcon = GetIcon(testResults.Select(r => r.Outcome).ToArray());
                var fileName = resultFile.Name;

                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("<details>");
                stringBuilder.AppendLine("<summary>");
                stringBuilder.AppendLine($"<strong>{fileIcon} {fileName}</strong>");
                stringBuilder.AppendLine("</summary>");
                stringBuilder.AppendLine();

                stringBuilder.AppendLine("|     | TestName | Time | Message |");
                stringBuilder.AppendLine("| :-: | :------: | :--: | ------- |");

                foreach (var testResult in testResults)
                {
                    var icon = GetIcon(testResult.Outcome);
                    var testName = testResult.TestName;
                    var totalSeconds = TimeSpan.Parse(testResult.Duration).TotalSeconds;
                    var message = GetMessage(testResult);
                    stringBuilder.AppendLine($"| {icon} | {testName} | {totalSeconds:0.00} | {message} |");
                }

                stringBuilder.AppendLine();
                stringBuilder.AppendLine("<summary>");
                stringBuilder.AppendLine();

                return stringBuilder.ToString();
            }

            const string IconFailed = ":x:";
            const string IconSkipped = ":warning:";
            const string IconPassed = ":heavy_check_mark:";
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
            /// GetMessage
            /// </summary>
            /// <param name="testResult"></param>
            /// <returns></returns>
            public static string GetMessage(UnitTestResult testResult)
            {
                var message = $"{testResult.Output?.StdOut}\n{testResult.Output?.StdErr}\n{testResult.Output?.ErrorInfo?.Message}\n{testResult.Output?.ErrorInfo?.StackTrace}";
                message = message.Trim();
                return message.Replace("\r", "").Replace("\n", "<br>");
            }
        }
        /// <summary>
        /// GetTestFileResults
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IEnumerable<UnitTestResult> GetTestFileResults(AbsolutePath file)
        {
            var serializer = new XmlSerializer(typeof(UnitTestResult));
            var elements = GetTestFileElements(file);
            return elements.Select(e => (UnitTestResult)serializer.Deserialize(e.CreateReader()));
        }

        #region GetTestFile
        /// <summary>
        /// GetTestFileOutcomes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTestFileOutcomes(AbsolutePath file)
            => XmlTasks.XmlPeek(
                file,
                "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));

        /// <summary>
        /// GetTestFileDurations
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IEnumerable<TimeSpan> GetTestFileDurations(AbsolutePath file)
            => XmlTasks.XmlPeek(
                file,
                "/xn:TestRun/xn:Results/xn:UnitTestResult/@duration",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010")).Select(TimeSpan.Parse);

        /// <summary>
        /// GetTestFileElements
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IEnumerable<XElement> GetTestFileElements(AbsolutePath file)
            => XmlTasks.XmlPeekElements(
                file,
                "/xn:TestRun/xn:Results/xn:UnitTestResult",
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));

        #endregion
    }
}