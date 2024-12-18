﻿using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Utilities.Collections;
using ricaun.Nuke.Extensions;
using ricaun.Nuke.Tools;
using System;

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

        internal static bool _waningSignFile = false;
        internal void WarningSignFile(string message)
        {
            if (_waningSignFile) return;
            _waningSignFile = true;
            Serilog.Log.Warning(message);
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
            if (!SignFile.SkipEmpty())
            {
                WarningSignFile("SignFile is empty, SignFolder will be skipped. Configure environment variables 'SIGN_FILE' and 'SIGN_PASSWORD'.");
                return false;
            }
            if (!SignPassword.SkipEmpty())
            {
                WarningSignFile("SignPassword is empty, SignFolder will be skipped. Configure environment variables 'SIGN_FILE' and 'SIGN_PASSWORD'.");
                return false;
            }

            Action<string> signFile = null;

            if (AzureKeyVaultConfig.Create(SignFile) is AzureKeyVaultConfig azureKeyVaultConfig)
            {
                var azureKeyVaultClientSecret = SignPassword;
                void SignUsingAzureKeyVault(string file)
                {
                    AzureSignToolUtils.Sign(file, azureKeyVaultConfig, azureKeyVaultClientSecret);
                }
                signFile = SignUsingAzureKeyVault;
            }
            else
            {
                var certPath = SignExtension.VerifySignFile(SignFile, BuildAssemblyDirectory);
                var certPassword = SignPassword;

                SignExtension.CreateCerFile(certPath, certPassword, BuildAssemblyDirectory);

                void SignUsingCerFile(string file)
                {
                    SignExtension.Sign(certPath, certPassword, file);
                }
                signFile = SignUsingCerFile;
            }

            Serilog.Log.Information($"SignFolder [{namePattern}]: {folder}");

            if (dllSign)
                Globbing.GlobFiles(folder, $"**/{namePattern}.dll")
                    .ForEach(signFile);

            if (nupkgSign)
                Globbing.GlobFiles(folder, $"**/{namePattern}.nupkg")
                    .ForEach(signFile);

            if (exeSign)
                Globbing.GlobFiles(folder, $"**/{namePattern}.exe")
                    .ForEach(signFile);

            return Globbing.GlobFiles(folder, $"**/{namePattern}").Count > 0;
        }
    }
}
