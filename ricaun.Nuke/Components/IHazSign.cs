using Nuke.Common;
using Nuke.Common.ValueInjection;

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
        [Secret] [Parameter] public string SignFile => ValueInjectionUtility.TryGetValue(() => SignFile);

        /// <summary>
        /// SignPassword
        /// </summary>
        [Secret] [Parameter] public string SignPassword => ValueInjectionUtility.TryGetValue(() => SignPassword);
    }
}
