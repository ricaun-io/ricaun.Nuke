using Nuke.Common;

namespace ricaun.Nuke
{
    public static class CommonExtension
    {
        public static T From<T>(this T nukeBuild) where T : INukeBuild => (T)(object)nukeBuild;
    }
}

