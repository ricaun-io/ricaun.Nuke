using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ricaun.Nuke.Extensions
{
    public static class GitHubExtension
    {
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
                Logger.Normal($"Added file: {file}");
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
        public static void ReleaseDraft(string gitHubOwner, string gitHubName, Release draft)
        {
            var _ = GitHubTasks.GitHubClient.Repository.Release
                .Edit(gitHubOwner, gitHubName, draft.Id, new ReleaseUpdate { Draft = false })
                .Result;
        }
        #endregion
    }
}
