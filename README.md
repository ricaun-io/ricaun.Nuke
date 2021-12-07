# ricaun.Nuke

[![Publish](https://github.com/ricaun-io/ricaun.Nuke/actions/workflows/Publish.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Nuke/actions)
[![Develop](https://github.com/ricaun-io/ricaun.Nuke/actions/workflows/Develop.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Nuke/actions)

# Build

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
    string IHazContent.Folder => "Release";
    string IHazRelease.Folder => "Output";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
```

---

Copyright © 2021 ricaun