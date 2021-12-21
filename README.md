# ricaun.Nuke

This package is to simplify the build automation system using [Nuke.Common](https://www.nuget.org/packages/Nuke.Common/).

[![Publish](https://github.com/ricaun-io/ricaun.Nuke/actions/workflows/Publish.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Nuke/actions)
[![Develop](https://github.com/ricaun-io/ricaun.Nuke/actions/workflows/Develop.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Nuke/actions)
[![Release](https://img.shields.io/nuget/v/ricaun.Nuke?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.Nuke)

# Examples

# Build.cs - IPublish

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublish
{
    public static int Main() => Execute<Build>(x => x.From<IPublish>().Build);
}
```

# Build.cs - IPublishPack

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

[CheckBuildProjectConfigurations]
class Build : NukeBuild, IPublishPack
{
    // string IHazContent.Folder => "Release";
    // string IHazRelease.Folder => "ReleaseFiles";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
```

## License

This package is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this package? Please [star this project on GitHub](https://github.com/ricaun-io/ricaun.Nuke/stargazers)!

---

Copyright © 2021 ricaun