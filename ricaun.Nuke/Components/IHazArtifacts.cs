using System;
using Nuke.Common;
using Nuke.Common.IO;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    interface IHazArtifacts : IHazSolution, INukeBuild
    {
        AbsolutePath ArtifactsDirectory => RootDirectory / ".." / "bin" / "Artifacts";
    }
}
