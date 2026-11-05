
using Nuke.Common;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ricaun.Nuke.Injection;

internal static class NukeInjection
{
    public static void CreateInstance<T>(this T nukeBuild, Assembly assembly, Action<string> informationAction = null) where T : INukeBuild
    {
        if (Debugger.IsAttached && informationAction == null)
        {
            informationAction = (text) => Serilog.Log.Information(text);
        }

        try
        {
            var types = assembly.GetTypes()
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
        catch (Exception ex)
        {
            informationAction?.Invoke($"Failed to GetTypes of {assembly.FullName}: {ex.Message}");
        }
    }
}
