using Nuke.Common;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// ITest
    /// </summary>
    public interface ITest : IHazTest
    {
        /// <summary>
        /// TestResults (Default: true)
        /// </summary>
        [Parameter]
        bool TestResults => TryGetValue<bool?>(() => TestResults) ?? true;

        /// <summary>
        /// TestBuildStopWhenFailed (Default: true)
        /// </summary>
        [Parameter]
        bool TestBuildStopWhenFailed => TryGetValue<bool?>(() => TestBuildStopWhenFailed) ?? true;

        /// <summary>
        /// TestProjectName (Default: "*.Tests")
        /// </summary>
        [Parameter]
        string TestProjectName => TryGetValue<string>(() => TestProjectName) ?? "*.Tests";

        /// <summary>
        /// Test
        /// </summary>
        Target Test => _ => _
            .TriggeredBy(Compile)
            .Executes(() =>
            {
                TestProjects(TestProjectName, TestResults, TestBuildStopWhenFailed);
            });
    }
}