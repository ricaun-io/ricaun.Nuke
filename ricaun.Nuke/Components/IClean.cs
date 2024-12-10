using Nuke.Common;
using Nuke.Common.IO;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IClean
    /// </summary>
    public interface IClean : IHazSolution, INukeBuild
    {
        /// <summary>
        /// Target Clean
        /// </summary>
        Target Clean => _ => _
            .Executes(() =>
            {
                CreateTemporaryIgnore();
                Solution.ClearSolution(BuildProjectDirectory);
            });


        private void CreateTemporaryIgnore()
        {
            var tempIgnore = TemporaryDirectory / ".." / ".gitignore";
            if (!tempIgnore.FileExists())
                tempIgnore.WriteAllText("temp");
        }
    }
}
