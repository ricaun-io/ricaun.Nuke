using Nuke.Common;
using Nuke.Common.Utilities;
using ricaun.Nuke.Injection;
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
            nukeBuild.ShowVersion();
            return (T)(object)nukeBuild;
        }

        internal static void ShowVersion<T>(this T nukeBuild) where T : INukeBuild
        {
            Information();
            var assemblyName = typeof(CommonExtension).Assembly.GetName().Name;
            foreach (var assembly in System.AppDomain.CurrentDomain.GetAssemblies()
                .Where(e => e.GetName().Name.StartsWith(assemblyName, System.StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(e => e.GetName().Name))
            {
                Information($"{assembly.GetName().Name} {assembly.GetVersionText()}");
                NukeInjection.CreateInstance(nukeBuild, assembly);
            }
            NukeInjection.CreateInstance(nukeBuild, nukeBuild.GetType().Assembly);
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

