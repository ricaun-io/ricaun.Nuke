using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.ValueInjection;

namespace ricaun.Nuke.Components
{
    public interface IHazSolution : INukeBuild
    {
        [Required] [Solution] Solution Solution => ValueInjectionUtility.TryGetValue(() => Solution);
    }
}
