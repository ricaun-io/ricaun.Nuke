using Nuke.Common;

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
        [Secret] [Parameter] public string SignFile => TryGetValue(() => SignFile);

        /// <summary>
        /// SignPassword
        /// </summary>
        [Secret] [Parameter] public string SignPassword => TryGetValue(() => SignPassword);
    }
}
