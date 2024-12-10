# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](http://keepachangelog.com/en/1.0.0/)
and this project adheres to [Semantic Versioning](http://semver.org/spec/v2.0.0.html).

## [1.9.0] / 2024-12-06
### Features
- Enable sign files using `Azure Key Vault`.
### Build
- Add `IAzureSignTool` to check if `AzureSignToolUtils` is installed.
- Update `IAzureSignTool` with `Requires`.
- [ ] Add import `build` with `.targets`
### Updates
- Add `AzureSignToolUtils` to sign files using `AzureSignToolTasks` or `NuGetKeyVaultSignToolTasks`.
- Add `NuGetKeyVaultSignTool` for nuke version `8.*`.
- Add `AzureKeyVaultConfig` with json file with `Azure Key Vault` without secrets.
- [ ] Add `build` with `.targets` to install packages `AzureSignTool` and `NuGetKeyVaultSignTool`.
- Add `PackageDownload` to download `AzureSignTool` and `NuGetKeyVaultSignTool` on the fly.
- Update `IClear` with `CreateTemporaryIgnore`.
- Update `ISign` with `Requires`.
### Tests
- Update `NuGetExtensionTests`
- Add `AzureKeyVaultConfigTests`

## [1.8.2] / 2024-11-20
### Updates
- Update `Nuke.Common` to `8.1.4`.
- Update `FileSystemTasks.CopyFileToDirectory` to `AbsolutePathExtensions.CopyToDirectory`.
- Update `FileSystemTasks.CopyDirectoryRecursively` to `AbsolutePathExtensions.Copy`.
- Add `SymbolPackageFormat` and `IncludeSymbols` in the package.
- Update release `*.nupkg` to `*.*nupkg` to copy symbol package format. (Fix: #75)
- Add `WarningSignFile` to show warning when sign file is empty. (Fix: #74)
- Update build project with `Configuration` and `TargetFramework` in `Build` and `Rebuild`. (Fix: #73)

## [1.8.1] / 2024-05-13
### Updated
- Update `GetFileVersionInfo` to find `exe` file.
- Update `SignProject` to sign `Name`, `AssemblyName`, or `*`.
- Update `AssemblyExtension` methods to `Obsolete`.

## [1.8.0] / 2024-03-11
### Updated
- Update `Nuke.Common` Version = `8.0.0`
- Force enable `EnableUnsafeBinaryFormatterSerialization` to support `net8.0`.

## [1.7.4] / 2024-02-08
### Features
- Update `GetInformationalVersion` to find `nupkg` files.
### Added
- Add `NugetVersionInfo` to parse `nupkg` files by name.

## [1.7.3] / 2023-12-18
### Features
- `IPack` with `UnlistNuget` to unlist nuget package.

## [1.7.2] / 2023-10-05 - 2023-11-18
### Features
- Enable `SignBinary` with `PathTooLong` path.
### Updated
- Create `PathTooLongUtils` to prevent `PathTooLongException`
### Fix
- Add `+` split with fix `GetInformationalVersion` with `+` in version. (Fix: #67)

## [1.7.1] / 2023-10-05
### Features
- Prerelease Nuget force to unlist.
- PreReleaseFilter to filter pre-release versions.
### Updated
- Create `NuGetExtension` with methods for dotnet/nuget.
- Update `IPrePack` and `IGitPreRelease` to filter prerelease.
### Tests
- Test `TryGetPackageNameAndVersion`

## [1.7.0] / 2023-10-04
### Features
- Prerelease feature for Github and Nuget.
### Updated
- Update `AssemblyExtension` to fix `GetVersion` prerelease key work.
- Update `ITest` to runs before `Release`
- Update `IGitRelease` with custom method to create release and pre-release.
- Update `GitHubExtension` to release or pre-release.
- Update order of `Targets`
- Update `IGitRelease` to fail if MainProject is prerelease.
- Update `IHazPack` with base package methods.
### Added
- Add `IGitPreRelease` with custom method to create pre-release when version is pre-release.
- Add `IPrePack` with custom method to create pre-release when version is pre-release.

## [1.6.1] / 2023-10-02
### Fixed
- Fix test file with multi target framework (Fix: #60)
### Updated
- Update `LogfileName` to `LogFilePrefix` (Fix: #60)
### Tests
- Add `ricaun.Nuke.Tests` with multi target framework

## [1.6.0] / 2023-10-02
### Features
- Update `IHazExample` and `ICompileExample` (Fix: #59)
### Updated
- Change `IHazExample` Name to wildcard pattern `*.Example`
- Move `ISign` methods to `IHazSign`
- Change `IHazRelease` Folder to ReleaseFolder
- Add `ReportSummaryProjectNames` in `IHazExample`

## [1.5.2] / 2023-10-02
### Features
- TestResult with `TargetFramework` in name. (Fix: #60)
- `IPack` releases all packages in the `ReleaseFiles` folder. (Fix: #58)
### Updated
- Tests files with `TargetFramework` in name
- Update `Nuke.Common` version to `7.0.6`
- Update `AssemblyExtension` with `GetAssemblyLastCreated`
- Update `IHazTest` to make TestResults with `TargetFramework`
- Update `IPack` to releases all packages in the `ReleaseFiles` folder.
### Tests
- Add diferent `TargetFramework` in each configuration

## [1.5.1] / 2023-07-21 - 2023-07-24
### Features
- GitHub Actions with Test Summary
- `Utils` with `TestReportUtil` and `TestRunUtil`
### Updated
- Update `ITest` log file name with `ProjectName` + `Configuration`
- Update to dotnet `7.0` the `Build` project
- Update `IHazTest` folder to `TestResults`
- Update `IHazTest` remove `ICompile`
### Added
- Add `Utils` with `TestReportUtil` and `TestRunUtil`
- Add `IHazGitHubActions`
### Tests
- Update `Tests` with fail test, ignore and pass.
- Add configuration `Ignore` and `Fail`
- Add `TestCase` in configuration `Release`

## [1.5.0] / 2023-05-31
### Updated
- Update `Nuke.Common` Version = `7.0.2`
- Update `GetProjects` to `GetAllProjects`
- Update `Test` with Verbosity to Normal by default
### Fixed
- Fix Test Components without namespace

## [1.4.4] / 2023-03-30
### Updated
- IHazTest with `SetCustomDotNetTestSettings` 

## [1.4.3] / 2023-01-24
### Updated
- Update `IGitRelease` adding information
### Fixed
- Fix `GetChangelogFile` is null parent

## [1.4.2] / 2022-12-21
### Features
- ITestLocal - Run all projects with `*.Tests` only Local Build
- ITestServer - Run all projects with `*.Tests` only Server Build
### Updated
- Set `Nuke.Common` Version = `6.2.1`
### Added
- Add `IHazTest` with base tests methods.
- Add `DownloadFileRetry` in `HttpClientExtension`

## [1.4.1] / 2022-12-15
### Features
- ITest - Run all projects with `*.Tests` 
### Updated
- Update `SignExtension` - `throw new PathTooLongException` when length > 260
### Added
- Add project `ricaun.Nuke.Example.Tests`

## [1.4.0] / 2022-10-09
### Features
- Enable / Disable `ReleaseNameVersion` (default false)
### Updated
- Update `IHazRelease` add `ReleaseNameVersion`
- Update `IHazRelease` add `GetReleaseFileNameVersion`
- Update `IHazRelease` add `CreateReleaseFromDirectory`
- Update `Build` to target "Build"
- Update `BuildProject` and `RebuildProject`

## [1.3.6] / 2022-08-18
### Updated
- Update GetGitRepositoryOwner null Exception

## [1.3.5] / 2022-08-12
### Updated
- Update SignFile `base64`

## [1.3.4] / 2022-07-19
### Updated
- Update/Fix ReleaseNotes, GetReleaseNotes

## [1.3.3] / 2022-07-18
### Updated
- Update ReleaseNotes, GetReleaseNotes

## [1.3.2] / 2022-07-18
### Updated
- Show Error if `SignToolTasks.SignToolPath` is empty

## [1.3.1] / 2022-06-15
### Updated
- Set `Solution(SuppressBuildProjectCheck = true)`
- Update Package to net6.0
### Added
- Test Download
- HttpClientExtension

## [1.3.0] / 2022-06-14
### Fixed
- Fix `IHazContent` Documentation
### Updated
- Update to Visual Studio 2022 - net6.0
- Update Nuke Version
- Update `timestampServers` order
- Warning if ClearSolution fail

## [1.2.2] / 2022-04-22
### Bug Fixed
- GitRepository thrown error

## [1.2.1] / 2022-04-06
### Changed
- Sign Before Release

## [1.2.0] / 2022-04-04
### Features
- Add Sign multiple Timestamp Server

## [1.1.3] / 2022-03-10
### New Features
- Add SignFolder on ISign

## [1.1.2] / 2022-02-23
### Bug Fixes
- Null GetFileVersionInfo set Warning
### Changed
- Add GetAssemblyName
- Add GetComments & GetFileDescription
- Add ShowInformation On MainProject

## [1.1.1] / 2022-02-23
### New Features
- Compile Multiple Examples with EndWith `Name`
### Bug Fixes
- Fix Error When Read AssemblyAttribute
### Changed
- Add XmlnsDefinition on Example to Force Error with AssemblyAttribute
- Remove AssemblyAttribute
- Add RevitAddin Example
- Add Multiple Example Compile

## [1.1.0] / 2022-02-15
- Add GitVersion.CommandLine
- Remove PackageDownload GitVersion.Tool
- Add GitVersion.Tool
- Include="NuGet.CommandLine"
- Set Nuke.Common as *
- Remove ValueInjectionUtility
- Remove Logger to Serilog.Log
- Update to Nuke.Common 6.0.0 Version

## [1.0.2] / 2022-01-19
- Fix Version with 3 fields

## [1.0.1] / 2022-01-19
- Remove not used file `Build`
- Fix Version Information default value
- GetValue add defaulValue
- Find Changelog File
- Update `IHazChangelog` 

## [1.0.0] / 2022-01-19
- Update to public Repo

## [0.0.16] / 2022-01-11
- Fix IHazGitRepository - `GetGitRepositoryOwner`

## [0.0.15] / 2022-01-11
- Update Readme
- Make Simple Nuke - Remove EnvironmentInfo

## [0.0.14] / 2022-01-04
- Add `CreateCerFile` SignExtension
- Add `CreateCertificatesCer` SignExtension

## [0.0.13] / 2021-12-21
- Add `IHazExample.ReleaseExample`
- Change Readme

## [0.0.12] / 2021-12-21
- Add Documentation on the main project
- Add `xml` DocumentationFile
- Clear BuildExtension
- Add Components xml Comments
- Clear IHazChangelog
- Add HazMainProjectExtension `GetProject(string)`
- Change IHazContent Folder to `Release`
- Change IHazRelease Folder to `ReleaseFiles`
- Normalize Name Folder

## [0.0.11] / 2021-12-17
- Fix Zip Already Exists
- MainName Update

## [0.0.10] / 2021-12-17
- Change Base Release `.nupkg` or full folder
- Fix HazSolutionExtension
- Add GetMainProject on IHazMainProject 
- private GetMainProject
- Add HazSolutionExtension
- Add HazMainProjectExtension
- Add IHazMainProject
- Rename Example Folder Project

## [0.0.9] / 2021-12-17
- Clear Build
- Test Native Example Compile
- Rename Example Project
- Add ICompileExample
- Add IHazExample
- Add Native Example Compile

## [0.0.8] / 2021-12-17
- Release Example Project
- Add Example Project
- Add Select other Project To Compile and Release
- Add Example to Compile
- Set Visible false

## [0.0.7] / 2021-12-09
- Add Readme inside Package
- Update Readme

## [0.0.6] / 2021-12-09
- Add Zip Create Directory
- Remove extra icon
- Update Changelog
- Update Readme Add Label Nuget
- Remove Signing Warning

## [0.0.5] / 2021-12-08
- Change `NugetApiUrl: ${{ secrets.NUGET_API_URL }}`
- Change `NugetApiKey: ${{ secrets.NUGET_API_KEY }}`
- Change To `https://api.nuget.org/v3/index.json`

## [0.0.5] / 2021-12-08
- Fix Dll version name

## [0.0.4] / 2021-12-08
- Add FileVersion on Debug
- Add BuildOtherExtension
- Add AssemblyExtension

## [0.0.3] / 2021-12-07
- Update Develop.yml `branches-ignore:`
- Update to ReleasePack
- Update IRelease Add `exe` and remove Folder on Zip
- Update IHazRelease Remove Parameter

## [0.0.2] / 2021-12-07
- Update ReadMe
- Update Release Pack
- Remove Artifacts
- Update Changelog

## [0.0.1] / 2021-12-06
- Add ReleasePack
- Add Parameter Folder
- Fix LoadAssembly add try
- Github Develop
- Github Master
- Nuke Local Test - Ok
- Add LICENCE
- First Release

[vNext]: ../../compare/1.0.0...HEAD
[1.9.0]: ../../compare/1.8.2...1.9.0
[1.8.2]: ../../compare/1.8.1...1.8.2
[1.8.1]: ../../compare/1.8.0...1.8.1
[1.8.0]: ../../compare/1.7.4...1.8.0
[1.7.4]: ../../compare/1.7.3...1.7.4
[1.7.3]: ../../compare/1.7.2...1.7.3
[1.7.2]: ../../compare/1.7.1...1.7.2
[1.7.1]: ../../compare/1.7.0...1.7.1
[1.7.0]: ../../compare/1.6.1...1.7.0
[1.6.1]: ../../compare/1.6.0...1.6.1
[1.6.0]: ../../compare/1.5.2...1.6.0
[1.5.2]: ../../compare/1.5.1...1.5.2
[1.5.1]: ../../compare/1.5.0...1.5.1
[1.5.0]: ../../compare/1.4.4...1.5.0
[1.4.4]: ../../compare/1.4.3...1.4.4
[1.4.3]: ../../compare/1.4.2...1.4.3
[1.4.2]: ../../compare/1.4.1...1.4.2
[1.4.1]: ../../compare/1.4.0...1.4.1
[1.4.0]: ../../compare/1.3.5...1.4.0
[1.3.6]: ../../compare/1.3.5...1.3.6
[1.3.5]: ../../compare/1.3.4...1.3.5
[1.3.4]: ../../compare/1.3.3...1.3.4
[1.3.3]: ../../compare/1.3.2...1.3.3
[1.3.2]: ../../compare/1.3.1...1.3.2
[1.3.1]: ../../compare/1.3.0...1.3.1
[1.3.0]: ../../compare/1.2.2...1.3.0
[1.2.2]: ../../compare/1.2.1...1.2.2
[1.2.1]: ../../compare/1.2.0...1.2.1
[1.2.0]: ../../compare/1.1.3...1.2.0
[1.1.3]: ../../compare/1.1.2...1.1.3
[1.1.2]: ../../compare/1.1.1...1.1.2
[1.1.1]: ../../compare/1.1.0...1.1.1
[1.1.0]: ../../compare/1.0.2...1.1.0
[1.0.2]: ../../compare/1.0.1...1.0.2
[1.0.1]: ../../compare/1.0.0...1.0.1
[1.0.0]: ../../compare/0.0.16...1.0.0
[0.0.16]: ../../compare/0.0.15...0.0.16
[0.0.15]: ../../compare/0.0.14...0.0.15
[0.0.14]: ../../compare/0.0.13...0.0.14
[0.0.13]: ../../compare/0.0.12...0.0.13
[0.0.12]: ../../compare/0.0.11...0.0.12
[0.0.11]: ../../compare/0.0.10...0.0.11
[0.0.10]: ../../compare/0.0.9...0.0.10
[0.0.9]: ../../compare/0.0.8...0.0.9
[0.0.8]: ../../compare/0.0.7...0.0.8
[0.0.7]: ../../compare/0.0.6...0.0.7
[0.0.6]: ../../compare/0.0.5...0.0.6
[0.0.5]: ../../compare/0.0.4...0.0.5
[0.0.4]: ../../compare/0.0.3...0.0.4
[0.0.3]: ../../compare/0.0.2...0.0.3
[0.0.2]: ../../compare/0.0.1...0.0.2
[0.0.1]: ../../compare/0.0.1