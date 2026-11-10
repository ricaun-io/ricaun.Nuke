using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitHub;
using ricaun.Nuke.Components;
using ricaun.Nuke.Extensions;

namespace ricaun.Nuke.Injection;

internal class NuGetAddGitHubSourceInjection : INukeInjection
{
    public NuGetAddGitHubSourceInjection(INukeBuild nukeBuild, IHazGitRepository gitRepository)
    {
        if (gitRepository.GitRepository is null)
            return;

        if (nukeBuild.IsLocalBuild)
            return;

        var repository_owner = gitRepository.GetGitRepositoryOwner();
        var userName = gitRepository.GetGitRepositoryPackageUser();
        var password = gitRepository.GitHubToken;

        NuGetExtension.DotNetNuGetAddGitHubSource(repository_owner, userName, password);
    }
}