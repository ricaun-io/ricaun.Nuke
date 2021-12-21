using Nuke.Common;

namespace ricaun.Nuke
{
    /// <summary>
    /// CommonExtension
    /// </summary>
    public static class CommonExtension
    {
        /// <summary>
        /// From <see cref="INukeBuild"/> to <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nukeBuild"></param>
        /// <returns></returns>
        public static T From<T>(this T nukeBuild) where T : INukeBuild => (T)(object)nukeBuild;
    }
}

