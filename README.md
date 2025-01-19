# ricaun.Nuke

This package is to simplify the build automation system using [Nuke.Common](https://www.nuget.org/packages/Nuke.Common/).

[![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue)](../..)
[![Nuke](https://img.shields.io/badge/Nuke-Build-blue)](https://nuke.build/)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![Build](https://github.com/ricaun-io/ricaun.Nuke/actions/workflows/Build.yml/badge.svg)](https://github.com/ricaun-io/ricaun.Nuke/actions)
[![Release](https://img.shields.io/nuget/v/ricaun.Nuke?logo=nuget&label=release&color=blue)](https://www.nuget.org/packages/ricaun.Nuke)

# Examples

# Build.cs - IPublish

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublish
{
    // string IHazMainProject.MainName => "ProjectName";
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
    // string IHazMainProject.MainName => "ProjectName";
    public static int Main() => Execute<Build>(x => x.From<IPublishPack>().Build);
}
```

## Environment Variables

### Publish Github

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
```

### SignFile and SignPassword

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
    SignFile: ${{ secrets.SIGN_FILE }}
    SignPassword: ${{ secrets.SIGN_PASSWORD }}
```

`SignFile` could be a `file/url/Base64` to the certificate file. 
`SignPassword` is the password to the certificate file.

#### SignFile using `Azure Key Vault`

To simplify the configuration to sign with `Azure Key Vault` using the same environment variables are used `SignFile` and `SignPassword`.

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
    SignFile: ${{ secrets.SIGN_FILE_AZURE }}
    SignPassword: ${{ secrets.SIGN_PASSWORD_AZURE }}
```

##### SIGN_FILE_AZURE

The `SIGN_FILE_AZURE` is a `json` with the base configuration of the certificated in the `Azure Key Vault`:

```json
{
    "AzureKeyVaultCertificate": "AzureKeyVaultCertificate",
    "AzureKeyVaultUrl": "AzureKeyVaultUrl",
    "AzureKeyVaultClientId": "AzureKeyVaultClientId",
    "AzureKeyVaultTenantId": "AzureKeyVaultTenantId",
    "TimestampUrl" : "http://timestamp.digicert.com"
    "TimestampDigest" : "sha256"
}
```

The `TimestampUrl` and `TimestampDigest` are optional.

##### SIGN_PASSWORD_AZURE

The `SIGN_PASSWORD_AZURE` is the `AzureKeyVaultClientSecret` of the `Azure Key Vault` certificate.

### Publish Package NuGet

```yml
env:
    GitHubToken: ${{ secrets.GITHUB_TOKEN }}
    SignFile: ${{ secrets.SIGN_FILE }}
    SignPassword: ${{ secrets.SIGN_PASSWORD }}
    NuGetApiUrl: ${{ secrets.NUGET_API_URL }}
    NuGetApiKey: ${{ secrets.NUGET_API_KEY }}
```

# Build.cs - ITest

`ITest` runs all the `TestLocalProjectName` tests on local build and server build.

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublish, ITest
{
    // bool ITest.TestBuildStopWhenFailed => true;
    // string ITest.TestProjectName => "*.Tests";
    public static int Main() => Execute<Build>(x => x.From<IPublish>().Build);
}
```

## Build.cs - ITestLocal

`ITestLocal` runs all the `TestLocalProjectName` tests only on local build.

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublish, ITestLocal
{
    // bool ITestLocal.TestLocalBuildStopWhenFailed => true;
    // string ITestLocal.TestLocalProjectName => "*.Tests";
    public static int Main() => Execute<Build>(x => x.From<IPublish>().Build);
}
```

## Build.cs - ITestServer

`ITestServer` runs all the `TestServerProjectName` tests only on server build.

```C#
using Nuke.Common;
using Nuke.Common.Execution;
using ricaun.Nuke;
using ricaun.Nuke.Components;

class Build : NukeBuild, IPublish, ITestServer
{
    // bool ITestServer.TestServerBuildStopWhenFailed => true;
    // string ITestServer.TestServerProjectName => "*.Tests";
    public static int Main() => Execute<Build>(x => x.From<IPublish>().Build);
}
```

## License

This package is [licensed](LICENSE) under the [MIT License](https://en.wikipedia.org/wiki/MIT_License).

---

Do you like this package? Please [star this project on GitHub](../../stargazers)!

---

Copyright © 2021 ricaun