using Nuke.Common;
using Nuke.Common.Utilities;
using System.Linq;

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
        public static T From<T>(this T nukeBuild) where T : INukeBuild
        {
            ShowVersion();
            return (T)(object)nukeBuild;
        }

        internal static void ShowVersion()
        {
            Information();
            var assemblyName = typeof(CommonExtension).Assembly.GetName().Name;
            foreach (var item in System.AppDomain.CurrentDomain.GetAssemblies()
                .Where(e => e.GetName().Name.StartsWith(assemblyName, System.StringComparison.InvariantCultureIgnoreCase)))
            {
                Information($"{item.GetName().Name} {item.GetVersionText()}");
            }
        }

        internal static void Information(string text = null)
        {
            try
            {
                // internal static void Information(string text = null)
                typeof(Host).GetMethod("Information",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    .Invoke(null, new object[] { text });
            }
            catch { };
        }
    }
}

