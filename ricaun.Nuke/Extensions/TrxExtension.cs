using Nuke.Common.IO;
using System.Collections.Generic;
using System.Linq;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// Test File (.trx) Extension
    /// </summary>
    public static class TrxExtension
    {
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
            var outcomes = TestResultUtil.GetTestFileOutcomes(testFile).ToList();
            var passedTests = outcomes.Count(x => x == TestResultUtil.TEST_OUTCOME_PASSED);
            var failedTests = outcomes.Count(x => x == TestResultUtil.TEST_OUTCOME_FAILED);
            var skippedTests = outcomes.Count(x => x == TestResultUtil.TEST_OUTCOME_SKIPPED);

            var totalSeconds = TestResultUtil.GetTestFileDurations(testFile).Select(e => e.TotalSeconds).Sum();

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

    }
}