using Nuke.Common;

namespace ricaun.Nuke.Components
{
    public interface IHazSign : INukeBuild
    {
        public string SignFile => EnvironmentInfo.GetVariable<string>("SIGN_FILE");
        public string SignPassword => EnvironmentInfo.GetVariable<string>("SIGN_PASSWORD");
    }
}
