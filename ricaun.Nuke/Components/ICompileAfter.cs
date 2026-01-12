using Nuke.Common;
using Nuke.Common.ProjectModel;
using System.Collections.Generic;

namespace ricaun.Nuke.Components;

/// <summary>
/// ICompileAfter
/// </summary>
public interface ICompileAfter : IHazCompileAfter, ISign, ICompile
{
    /// <summary>
    /// Target CompileAfter
    /// </summary>
    Target CompileAfter => _ => _
        .TriggeredBy(Compile)
        .Before(Sign)
        .Executes(() =>
        {
            ReportSummaryProjectNames(GetProjects());
            BuildProjectsAndRelease(GetProjects(), ReleaseCompile, ReleaseCompile, SignCompile);
        });
}

/// <summary>
/// IHazCompileAfter
/// </summary>
public interface IHazCompileAfter : IHazRelease
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
