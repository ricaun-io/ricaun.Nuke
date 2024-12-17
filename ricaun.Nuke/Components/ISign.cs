using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

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
            .Executes(() =>
            {
                SignProject(MainProject);
            });
    }
}
