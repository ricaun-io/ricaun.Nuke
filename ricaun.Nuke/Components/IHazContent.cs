using System;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
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
        AbsolutePath ContentDirectory => GetContentDirectory(Solution.GetMainProject());
        public AbsolutePath GetContentDirectory(Project project) => project.Directory / "bin" / Folder;
    }
}
