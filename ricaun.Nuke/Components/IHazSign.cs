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
        public string SignFile => EnvironmentInfo.GetVariable<string>("SIGN_FILE");

        /// <summary>
        /// SignPassword
        /// </summary>
        public string SignPassword => EnvironmentInfo.GetVariable<string>("SIGN_PASSWORD");
    }
}
