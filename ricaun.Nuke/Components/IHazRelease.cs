using System;
using Nuke.Common;
using Nuke.Common.IO;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazRelease : IHazSolution, INukeBuild
    {
        AbsolutePath ReleaseDirectory => Solution.GetMainProject().Directory / "bin" / "Release";
    }
}
