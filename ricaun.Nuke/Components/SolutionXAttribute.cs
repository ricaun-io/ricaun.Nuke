using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using System.Linq;
using System.Reflection;
using Nuke.Common.Utilities;
using System;

using ricaun.Nuke;

#if NET10_0_OR_GREATER
/// <summary>
/// Based in the souce code of the [SolutionAttribute] in Nuke.Common, but with some custom logic to support a "slnx" extension and to read the solution file path from a ".nuke" file if it exists.
/// </summary>
/// <param name="relativePath"></param>
/// <remarks>
/// Source Code: https://github.com/nuke-build/nuke/blob/develop/source/Nuke.Common/Attributes/SolutionAttribute.cs
/// </remarks>
[PublicAPI]
[UsedImplicitly(ImplicitUseKindFlags.Assign)]
internal class SolutionXAttribute(string relativePath) : ParameterAttribute(GetDescription(relativePath))
{
    private static string GetDescription(string relativePath)
    {
        return "Path to a solution file that is automatically loaded."
               + (relativePath != null ? $" Default is {relativePath}." : string.Empty);
    }

    public SolutionXAttribute()
        : this(relativePath: null)
    {
    }

    public override bool List { get; set; }
    public bool GenerateProjects { get; set; }

    public override object GetValue(MemberInfo member, object instance)
    {
        var solutionFile = TryGetSolutionFileFromNukeFile() ??
                           GetSolutionFileFromParametersFile(member);

        // Force to check if file exists, if does not exist, try with "x" suffix (e.g., "MySolution.slnx")
        if (!solutionFile.Exists())
        {
            var solutionXFile = solutionFile + "x";
            if (solutionXFile.Exists())
            {
                solutionFile = solutionXFile;
                CommonExtension.Information($"Solution file '{solutionXFile}' is found.");
            }
        }

        var deserializer = typeof(SolutionModelExtensions).GetMethods()
            .Single(x => x.Name == nameof(SolutionModelExtensions.ReadSolution) && x.ContainsGenericParameters)
            .MakeGenericMethod(member.GetMemberType());
        return ((Solution)deserializer.Invoke(obj: null, [solutionFile])).NotNull();
    }

    // TODO: allow wildcard matching? [Solution("nuke-*.sln")] -- no globbing?
    // TODO: for just [Solution] without parameter being passed, do wildcard search?
    private AbsolutePath GetSolutionFileFromParametersFile(MemberInfo member)
    {
        return relativePath != null
            ? Build.RootDirectory / relativePath
            : GetParameter<AbsolutePath>(member).NotNull($"No solution file defined for '{member.Name}'.");
    }

    // Reflectively call ParameterService.GetParameter<T>(MemberInfo), the method is internal.
    internal T GetParameter<T>(MemberInfo member)
    {
        //return ParameterService.GetParameter<T>(member, destinationType);
        var type = typeof(NukeBuild).Assembly.GetType("Nuke.Common.ParameterService");
        if (type == null)
        {
            throw new InvalidOperationException("Could not find type 'ParameterService' via reflection.");
        }

        var method = type
            .GetMethods()
            .FirstOrDefault(x => x.Name == "GetParameter" && x.IsGenericMethod && x.GetParameters().Length >= 1 && x.GetParameters()[0].ParameterType == typeof(MemberInfo));

        if (method == null)
        {
            throw new InvalidOperationException("Could not find method 'ParameterService.GetParameter<T>(MemberInfo)' via reflection.");
        }

        var genericMethod = method.MakeGenericMethod(typeof(T));

        var parameters = method.GetParameters();
        if (parameters.Length == 1)
        {
            return (T)genericMethod.Invoke(obj: null, new object[] { member });
        }
        else if (parameters.Length == 2)
        {
            return (T)genericMethod.Invoke(obj: null, new object[] { member, null });
        }
        else
        {
            throw new InvalidOperationException("The 'ParameterService.GetParameter<T>(MemberInfo)' method has an unexpected signature.");
        }
    }

    // Copy from Nuke.Common.Constants
    internal const string NukeFileName = NukeDirectoryName;
    internal const string NukeDirectoryName = ".nuke";

    private AbsolutePath TryGetSolutionFileFromNukeFile()
    {
        var nukeFile = Build.RootDirectory / NukeFileName;
        if (!nukeFile.Exists())
            return null;

        var solutionFileRelative = nukeFile.ReadAllLines().ElementAtOrDefault(0);
        Assert.True(solutionFileRelative != null && !solutionFileRelative.Contains(value: '\\'),
            $"First line of {NukeFileName} must provide solution path using UNIX separators");

        var solutionFile = Build.RootDirectory / solutionFileRelative;
        Assert.FileExists(solutionFile, $"Solution file '{solutionFile}' provided via {NukeFileName} does not exist");

        return solutionFile;
    }
}
#endif
