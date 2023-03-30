using Nuke.Common.Tools.DotNet;
using System;

namespace ricaun.Nuke.Extensions
{
    /// <summary>
    /// DotNetTestSettingsExtensions
    /// </summary>
    public static class DotNetTestSettingsExtensions
    {
        /// <summary>
        /// Set Test Settings with <paramref name="customDotNetTestSettings"/>
        /// </summary>
        /// <param name="dotNetTestSettings"></param>
        /// <param name="customDotNetTestSettings"></param>
        /// <returns></returns>
        public static DotNetTestSettings SetCustomDotNetTestSettings(
            this DotNetTestSettings dotNetTestSettings,
            Func<DotNetTestSettings, DotNetTestSettings> customDotNetTestSettings)
        {
            if (customDotNetTestSettings is null)
                return dotNetTestSettings;

            return customDotNetTestSettings.Invoke(dotNetTestSettings);
        }
    }
}
