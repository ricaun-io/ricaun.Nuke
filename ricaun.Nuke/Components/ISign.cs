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
        Target Sign => _ => _
            .TriggeredBy(Compile)
            .Executes(() =>
            {
                SignProject(MainProject);
            });

        /// <summary>
        /// Sing All Files On the Project Folder
        /// </summary>
        /// <param name="project"></param>
        public void SignProject(Project project)
        {
            var projectFolder = project.Directory / "bin";

            if (!SignFile.SkipEmpty()) return;
            if (!SignPassword.SkipEmpty()) return;

            var certPath = SignExtension.VerifySignFile(SignFile, BuildAssemblyDirectory);
            var certPassword = SignPassword;

            var files = PathConstruction.GlobFiles(projectFolder, "**/*.dll");
            files.ForEach(file => SignExtension.SignBinary(certPath, certPassword, file));

            var nupkgs = PathConstruction.GlobFiles(projectFolder, "**/*.nupkg");
            nupkgs.ForEach(file => SignExtension.SignNuGet(certPath, certPassword, file));

            var exes = PathConstruction.GlobFiles(projectFolder, "**/*.exe");
            exes.ForEach(file => SignExtension.SignBinary(certPath, certPassword, file));
        }
    }
}
