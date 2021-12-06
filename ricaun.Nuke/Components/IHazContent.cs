using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ValueInjection;
using ricaun.Nuke.Extensions;
namespace ricaun.Nuke.Components
{
    public interface IHazContent : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Folder Content 
        /// </summary>
        [Parameter]
        string Folder => ValueInjectionUtility.TryGetValue(() => Folder) ?? "Content";
        AbsolutePath ContentDirectory => Solution.GetMainProject().Directory / "bin" / Folder;
    }
}
