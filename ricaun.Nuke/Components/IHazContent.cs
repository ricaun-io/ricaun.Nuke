using System;
using Nuke.Common;
using Nuke.Common.IO;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazContent : IHazSolution, INukeBuild
    {
        AbsolutePath ContentDirectory => Solution.GetMainProject().Directory / "bin" / "Content";
    }
}
