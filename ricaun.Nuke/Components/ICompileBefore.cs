using Nuke.Common;
using Nuke.Common.ProjectModel;
using System.Collections.Generic;

namespace ricaun.Nuke.Components;

/// <summary>
/// ICompileBefore
/// </summary>
public interface ICompileBefore : IHazCompileBefore, ICompile
{
    /// <summary>
    /// Target CompileBefore
    /// </summary>
    Target CompileBefore => _ => _
        .TriggeredBy(Clean)
        .Before(Compile)
        .Executes(() =>
        {
            ReportSummaryProjectNames(GetProjects());
            BuildProjectsAndRelease(GetProjects(), ReleaseCompile, ReleaseCompile, SignCompile);
        });
}

/// <summary>
/// IHazCompileBefore
/// </summary>
public interface IHazCompileBefore : IHazRelease
{
    /// <summary>
    /// Example Project matching a wildcard pattern (*.Example)
    /// </summary>
    [Parameter]
    string Name => TryGetValue(() => Name) ?? "*.Example";

    /// <summary>
    /// ReleaseCompile (default: true)
    /// </summary>
    [Parameter]
    bool ReleaseCompile => TryGetValue<bool?>(() => ReleaseCompile) ?? true;

    /// <summary>
    /// SignCompile (default: true)
    /// </summary>
    [Parameter]
    bool SignCompile => TryGetValue<bool?>(() => SignCompile) ?? true;

    /// <summary>
    /// GetProjects
    /// </summary>
    /// <returns></returns>
    public IEnumerable<Project> GetProjects() => Solution.GetAllProjects(Name);
}
