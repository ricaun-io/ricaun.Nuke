# ricaun.Nuke

This package is to simplify the build automation system using [Nuke.Common](https://www.nuget.org/packages/Nuke.Common/).

[![Visual Studio 2019](https://img.shields.io/badge/Visual%20Studio%202019-16.11.7+-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Publish](../../actions/workflows/Publish.yml/badge.svg)](../../actions)
[![Develop](../../actions/workflows/Develop.yml/badge.svg)](../../actions)
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

## Environment Variables

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
    SignFile: ${{ secrets.SIGN_FILE }}
    SignPassword: ${{ secrets.SIGN_PASSWORD }}
```

# Build.cs - IPublishPack

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublishPack
{
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
```

## Environment Variables

### Publish Package Github

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
    SignFile: ${{ secrets.SIGN_FILE }}
    SignPassword: ${{ secrets.SIGN_PASSWORD }}
```

### Publish Package Nuget

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
    SignFile: ${{ secrets.SIGN_FILE }}
    SignPassword: ${{ secrets.SIGN_PASSWORD }}
    NugetApiUrl: ${{ secrets.NUGET_API_URL }}
    NugetApiKey: ${{ secrets.NUGET_API_KEY }}
```


## License

This package is [licensed](LICENSE) under the [MIT Licence](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this package? Please [star this project on GitHub](../../stargazers)!

---

Copyright © 2021 ricaun