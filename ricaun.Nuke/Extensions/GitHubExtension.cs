using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// GitHubExtension
    /// </summary>
    public static class GitHubExtension
    {
        /// <summary>
        /// Determines whether the specified Git repository is a fork.
        /// </summary>
        /// <param name="gitRepository">The Git repository to check.</param>
        /// <returns>
        ///   <c>true</c> if the specified Git repository is a fork; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsForked(this GitRepository gitRepository)
        {
            if (gitRepository is null)
                return false;

            try
            {
                var gitHubOwner = gitRepository.GetGitHubOwner();
                var gitHubName = gitRepository.GetGitHubName();
                var repository = GitHubTasks.GitHubClient.Repository
                    .Get(gitHubOwner, gitHubName)
                    .Result;

                return repository.Fork;
            }
            catch
            {
                // Private repository is not forked.
                return false;
            }
        }

        #region GitHubUtil
        /// <summary>
        /// Check if Tags already exists.
        /// </summary>
        /// <param name="gitHubOwner"></param>
        /// <param name="gitHubName"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool CheckTags(string gitHubOwner, string gitHubName, string version)
        {
            var gitHubTags = GitHubTasks.GitHubClient.Repository
                .GetAllTags(gitHubOwner, gitHubName)
                .Result;

            if (gitHubTags.Select(tag => tag.Name).Contains(version))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Upload Release Files
        /// </summary>
        /// <param name="createdRelease"></param>
        /// <param name="files"></param>
        public static void UploadReleaseFiles(Release createdRelease, IEnumerable<AbsolutePath> files)
        {
            foreach (var file in files)
            {
                var releaseAssetUpload = new ReleaseAssetUpload
                {
                    ContentType = "application/x-binary",
                    FileName = Path.GetFileName(file),
                    RawData = File.OpenRead(file)
                };
                var _ = GitHubTasks.GitHubClient.Repository.Release.UploadAsset(createdRelease, releaseAssetUpload).Result;
                Serilog.Log.Information($"Added file: {file}");
            }
        }

        /// <summary>
        /// Create Draft
        /// </summary>
        /// <param name="gitHubOwner"></param>
        /// <param name="gitHubName"></param>
        /// <param name="newRelease"></param>
        /// <returns></returns>
        public static Release CreatedDraft(string gitHubOwner, string gitHubName, NewRelease newRelease) =>
            GitHubTasks.GitHubClient.Repository.Release
                .Create(gitHubOwner, gitHubName, newRelease)
                .Result;

        /// <summary>
        /// ReleaseDraft
        /// </summary>
        /// <param name="gitHubOwner"></param>
        /// <param name="gitHubName"></param>
        /// <param name="draft"></param>
        /// <param name="prerelease"></param>
        public static void ReleaseDraft(string gitHubOwner, string gitHubName, Release draft, bool prerelease = false)
        {
            var _ = GitHubTasks.GitHubClient.Repository.Release
                .Edit(gitHubOwner, gitHubName, draft.Id, new ReleaseUpdate { Draft = false, Prerelease = prerelease })
                .Result;
        }
        #endregion
    }
}
