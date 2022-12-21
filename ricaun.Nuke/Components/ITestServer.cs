using Nuke.Common;
/// <summary>
/// ITestServer
/// </summary>
public interface ITestServer : IHazTest
{
    /// <summary>
    /// TestServerResults (Default: true)
    /// </summary>
    [Parameter]
    bool TestServerResults => TryGetValue<bool?>(() => TestServerResults) ?? true;

    /// <summary>
    /// TestServerBuildStopWhenFailed (Default: true)
    /// </summary>
    [Parameter]
    bool TestServerBuildStopWhenFailed => TryGetValue<bool?>(() => TestServerBuildStopWhenFailed) ?? true;

    /// <summary>
    /// TestServerProjectName (Default: "*.Tests")
    /// </summary>
    [Parameter]
    string TestServerProjectName => TryGetValue<string>(() => TestServerProjectName) ?? "*.Tests";

    /// <summary>
    /// TestServer
    /// </summary>
    Target TestServer => _ => _
        .TriggeredBy(Compile)
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            TestProjects(TestServerProjectName, TestServerResults, TestServerBuildStopWhenFailed);
        });
}
