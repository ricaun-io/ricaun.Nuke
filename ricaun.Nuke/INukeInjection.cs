
using Nuke.Common;
using System;
using System.Linq;
using System.Reflection;

namespace ricaun.Nuke;

/// <summary>
/// Defines a contract for Nuke injection functionality.
/// </summary>
public interface INukeInjection
{

}

internal static class NukeInjection
{
    public static void CreateInstance<T>(this T nukeBuild, Assembly assembly, Action<string> informationAction = null) where T : INukeBuild
    {
        var types = assembly.GetExportedTypes()
            .Where(type => typeof(INukeInjection).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        foreach (var type in types)
        {
            try
            {
                var constructor = type.GetConstructors().FirstOrDefault();
                var arguments = constructor.GetParameters().Select(p =>
                {
                    if (p.ParameterType.IsInstanceOfType(nukeBuild))
                        return (object)nukeBuild;

                    return null;
                });
                informationAction?.Invoke($"Creating instance of {type.FullName} - {constructor}");
                var nukeInjection = constructor.Invoke(arguments.ToArray()) as INukeInjection;
            }
            catch (Exception ex)
            {
                informationAction?.Invoke($"Failed to create instance of {type.FullName}: {ex.Message}");
            }
        }
    }
}
