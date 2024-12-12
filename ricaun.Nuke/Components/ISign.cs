using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.AzureSignTool;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using ricaun.Nuke.Tools.NuGetKeyVaultSignTool;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// ISign
    /// </summary>
    public interface ISign : ICompile, IHazSign, IHazSolution, INukeBuild
    {
        /// <summary>
        /// Target Sign
        /// </summary>
        Target Sign => _ => _
            .TriggeredBy(Compile)
            //.Requires<NuGetKeyVaultSignToolTasks>()
            //.Requires<AzureSignToolTasks>()
            .Executes(() =>
            {
                SignProject(MainProject);
            });
    }
}
