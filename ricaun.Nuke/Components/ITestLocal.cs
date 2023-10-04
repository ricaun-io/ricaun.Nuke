using Nuke.Common;
namespace ricaun.Nuke.Components
{
    /// <summary>
    /// ITestLocal
    /// </summary>
    public interface ITestLocal : ICompile, IRelease, IHazTest
    {
        /// <summary>
        /// TestLocalResults (Default: true)
        /// </summary>
        [Parameter]
        bool TestLocalResults => TryGetValue<bool?>(() => TestLocalResults) ?? true;

        /// <summary>
        /// TestLocalBuildStopWhenFailed (Default: true)
        /// </summary>
        [Parameter]
        bool TestLocalBuildStopWhenFailed => TryGetValue<bool?>(() => TestLocalBuildStopWhenFailed) ?? true;

        /// <summary>
        /// TestLocalProjectName (Default: "*.Tests")
        /// </summary>
        [Parameter]
        string TestLocalProjectName => TryGetValue<string>(() => TestLocalProjectName) ?? "*.Tests";

        /// <summary>
        /// TestLocal
        /// </summary>
        Target TestLocal => _ => _
            .TriggeredBy(Compile)
            .Before(Release)
            .OnlyWhenStatic(() => IsLocalBuild)
            .Executes(() =>
            {
                TestProjects(TestLocalProjectName, TestLocalResults, TestLocalBuildStopWhenFailed);
            });
    }
}
