﻿using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazSign
    /// </summary>
    public interface IHazSign : INukeBuild
    {
        /// <summary>
        /// SignFile
        /// </summary>
        [Secret][Parameter] public string SignFile => TryGetValue(() => SignFile);

        /// <summary>
        /// SignPassword
        /// </summary>
        [Secret][Parameter] public string SignPassword => TryGetValue(() => SignPassword);

        /// <summary>
        /// Sing All Files On the Project bin Folder
        /// </summary>
        /// <param name="project"></param>
        public bool SignProject(Project project)
        {
            var projectFolder = project.Directory / "bin";
            return SignFolder(projectFolder, $"*{project.Name}*") || SignFolder(projectFolder, $"*{project.GetAssemblyName()}*") || SignFolder(projectFolder);
        }

        /// <summary>
        /// Sign Files on the Folder
        /// </summary>
        /// <param name="folder"></param>
        /// <param name="namePattern"></param>
        /// <param name="dllSign"></param>
        /// <param name="nupkgSign"></param>
        /// <param name="exeSign"></param>
        /// <returns></returns>
        public bool SignFolder(string folder, string namePattern = "*", bool dllSign = true, bool nupkgSign = true, bool exeSign = true)
        {
            if (!SignFile.SkipEmpty()) return false;
            if (!SignPassword.SkipEmpty()) return false;

            var certPath = SignExtension.VerifySignFile(SignFile, BuildAssemblyDirectory);
            var certPassword = SignPassword;

            SignExtension.CreateCerFile(certPath, certPassword, BuildAssemblyDirectory);

            Serilog.Log.Information($"SignFolder [{namePattern}]: {folder}");

            if (dllSign)
                Globbing.GlobFiles(folder, $"**/{namePattern}.dll")
                    .ForEach(file => SignExtension.SignBinary(certPath, certPassword, file));

            if (nupkgSign)
                Globbing.GlobFiles(folder, $"**/{namePattern}.nupkg")
                    .ForEach(file => SignExtension.SignNuGet(certPath, certPassword, file));

            if (exeSign)
                Globbing.GlobFiles(folder, $"**/{namePattern}.exe")
                    .ForEach(file => SignExtension.SignBinary(certPath, certPassword, file));

            return Globbing.GlobFiles(folder, $"**/{namePattern}").Count > 0;
        }
    }
}
