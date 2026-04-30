using Nuke.Common;
using System.Collections.Generic;
using static ricaun.Nuke.Extensions.BuildExtension;

namespace ricaun.Nuke;

/// <summary>
/// Provides extension methods for INukeBuild to access predefined sets of <see cref="BuildTool"/> instances.
/// These methods forward to the corresponding <see cref="BuildTools"/> helper methods.
/// </summary>
public static class BuildToolsExtension
{
    /// <summary>
    /// Returns a collection of <see cref="BuildTool"/> instances configured for MSBuild-only scenarios.
    /// </summary>
    /// <param name="build">The current <see cref="INukeBuild"/> instance. This parameter is unused and only enables the extension-method syntax.</param>
    /// <returns>An <see cref="IList{BuildTool}"/> containing tools appropriate for MSBuild-only builds.</returns>
    public static IList<BuildTool> MSBuildOnly(this INukeBuild build)
    {
        return BuildTools.MSBuildOnly();
    }

    /// <summary>
    /// Returns a collection of <see cref="BuildTool"/> instances configured for dotnet-build-only scenarios.
    /// </summary>
    /// <param name="build">The current <see cref="INukeBuild"/> instance. This parameter is unused and only enables the extension-method syntax.</param>
    /// <returns>An <see cref="IList{BuildTool}"/> containing tools appropriate for dotnet build-only workflows.</returns>
    public static IList<BuildTool> dotnetBuildOnly(this INukeBuild build)
    {
        return BuildTools.dotnetBuildOnly();
    }
}

