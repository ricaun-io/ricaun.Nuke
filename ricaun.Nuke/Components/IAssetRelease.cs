using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using ricaun.Nuke.IO;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// Represents the assets to be released.
    /// </summary>
    public class ReleaseAssets
    {
        /// <summary>
        /// Gets the project associated with the release.
        /// </summary>
        public Project Project { get; init; }

        /// <summary>
        /// Gets the version of the release.
        /// </summary>
        public string Version { get; init; }

        /// <summary>
        /// Gets the release notes.
        /// </summary>
        public string Notes { get; init; }

        /// <summary>
        /// Gets the collection of zip files to be released.
        /// </summary>
        public AbsolutePath[] Assets { get; init; } = new AbsolutePath[] { };

        /// <summary>
        /// Gets a value indicating whether the release is a prerelease.
        /// </summary>
        public bool Prerelease { get; init; } = false;
    }
    /// <summary>
    /// Defines a component that has asset release capabilities
    /// </summary>
    /// <remarks><see cref="IHazAssetRelease.AssetRelease"/> executes inside <see cref="IGitPreRelease"/> and <see cref="IGitRelease"/> before release.</remarks>
    public interface IHazAssetRelease
    {
        /// <summary>
        /// Gets the asset release instance.
        /// </summary>
        IAssetRelease AssetRelease => null;

        /// <summary>
        /// Releases the specified assets.
        /// </summary>
        /// <param name="releaseAssets">The assets to be released.</param>
        public void ReleaseAsset(ReleaseAssets releaseAssets)
        {
            if (AssetRelease is IAssetRelease assetRelease)
            {
                Serilog.Log.Information($"ReleaseAsset: {assetRelease}");
                assetRelease.ReleaseAsset(releaseAssets);
            }
        }
    }

    /// <summary>
    /// Defines the interface for releasing assets.
    /// </summary>
    public interface IAssetRelease
    {
        /// <summary>
        /// Releases the specified assets.
        /// </summary>
        /// <param name="releaseAssets">The assets to be released.</param>
        /// <remarks>Use <see cref="HttpAuthTasks"/> to post/put the release files in a private server.</remarks>
        public void ReleaseAsset(ReleaseAssets releaseAssets);
    }
}
