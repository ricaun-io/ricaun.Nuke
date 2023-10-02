using Nuke.Common.IO;
using Nuke.Common.Utilities.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.Nuke.Utils
{
    /// <summary>
    /// TestReport Utils for (.trx) Files
    /// </summary>
    public static class TestReportUtils
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

        #region TestReport
        /// <summary>
        /// TestReport
        /// </summary>
        public class TestReport
        {
            /// <summary>
            /// Passed
            /// </summary>
            public int Passed { get; set; }
            /// <summary>
            /// Failed
            /// </summary>
            public int Failed { get; set; }
            /// <summary>
            /// Skipped/NotExecuted
            /// </summary>
            public int Skipped { get; set; }
            /// <summary>
            /// TotalSeconds
            /// </summary>
            public double TotalSeconds { get; set; }
        }

        /// <summary>
        /// GetTestReport
        /// </summary>
        /// <param name="testFiles"></param>
        /// <returns></returns>
        public static TestReport GetTestReport(IEnumerable<AbsolutePath> testFiles)
        {
            if (testFiles.IsEmpty())
                return new TestReport();

            return testFiles.Select(GetTestReport).Aggregate((a, b) => new TestReport
            {
                Passed = a.Passed + b.Passed,
                Failed = a.Failed + b.Failed,
                Skipped = a.Skipped + b.Skipped,
                TotalSeconds = a.TotalSeconds + b.TotalSeconds
            });
        }

        /// <summary>
        /// GetTestReport
        /// </summary>
        /// <param name="testFile"></param>
        /// <returns></returns>
        public static TestReport GetTestReport(AbsolutePath testFile)
        {
            var outcomes = GetTestFileOutcomes(testFile).ToList();
            var passedTests = outcomes.Count(x => x == TEST_OUTCOME_PASSED);
            var failedTests = outcomes.Count(x => x == TEST_OUTCOME_FAILED);
            var skippedTests = outcomes.Count(x => x == TEST_OUTCOME_SKIPPED);

            var totalSeconds = GetTestFileTime(testFile).TotalSeconds;

            var testReport = new TestReport()
            {
                Passed = passedTests,
                Failed = failedTests,
                Skipped = skippedTests,
                TotalSeconds = totalSeconds,
            };
            return testReport;
        }
        #endregion

        #region GetTest
        /// <summary>
        /// GetTestFileOutcomes
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetTestFileOutcomes(AbsolutePath file)
            => XmlPeekTestRun(file, "/xn:TestRun/xn:Results/xn:UnitTestResult/@outcome");

        /// <summary>
        /// GetTestFileTime
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static TimeSpan GetTestFileTime(AbsolutePath file)
        {
            var start = XmlPeekTestRun(file, "/xn:TestRun/xn:Times/@start")
                .Select(DateTime.Parse)
                .FirstOrDefault();

            var finish = XmlPeekTestRun(file, "/xn:TestRun/xn:Times/@finish")
                .Select(DateTime.Parse)
                .FirstOrDefault();

            return finish - start;
        }

        /// <summary>
        /// XmlPeekTestRun
        /// </summary>
        /// <param name="file"></param>
        /// <param name="xpath"></param>
        /// <returns></returns>
        /// <remarks><paramref name="xpath"/> example: "/xn:TestRun/xn:Times/@start" </remarks>
        static IEnumerable<string> XmlPeekTestRun(AbsolutePath file, string xpath)
            => XmlTasks.XmlPeek(
                file,
                xpath,
                ("xn", "http://microsoft.com/schemas/VisualStudio/TeamTest/2010"));

        #endregion
    }
}