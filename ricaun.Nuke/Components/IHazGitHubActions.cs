using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;

namespace ricaun.Nuke.Components
{
    /// <summary>
    /// IHazGitHubActions
    /// </summary>
    public interface IHazGitHubActions
    {
        GitHubActions GitHubActions => GitHubActions.Instance;

        /// <summary>
        /// GitHubSummary
        /// </summary>
        /// <param name="messages"></param>
        void GitHubSummary(params object[] messages)
        {
            foreach (var message in messages)
            {
                Serilog.Log.Information($"GitHubSummaryFile: {message}");
                GitHubActions?.StepSummaryFile.AppendAllText($"{message}\n");
            }
        }
    }
}