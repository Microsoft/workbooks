# Xamarin Workbooks & Inspector

| macOS               | Windows             |
| ------------------- | ------------------- |
| ![][macbuildstatus] | ![][winbuildstatus] |

Xamarin Workbooks provide a blend of documentation and code that is perfect
for experimentation, learning, and creating guides and teaching aids.

Create a rich C# workbook for Android, iOS, Mac, or WPF, and get instant
live results as you learn these APIs.

The Inspector integrates with the app debugging workflow of your IDE,
serving as a debugging or diagnostics aid when inspecting your running app.

Live app inspection is available for enterprise customers.

## Resources

### Download

* [Download Latest Public Release for Mac](https://dl.xamarin.com/interactive/XamarinInteractive.pkg)
* [Download Latest Public Release for Windows](https://dl.xamarin.com/interactive/XamarinInteractive.msi)

### Documentation

* [Workbooks Documentation](https://developer.xamarin.com/guides/cross-platform/workbooks/)
* [Live Inspection Documentation](https://developer.xamarin.com/guides/cross-platform/inspector/)
* [Sample Workbooks](https://github.com/xamarin/Workbooks)

### Provide Feedback

* [Discuss in Xamarin Forums](https://forums.xamarin.com/categories/inspector)
* [File Bugs](https://bugzilla.xamarin.com/enter_bug.cgi?product=Workbooks%20%26%20Inspector)

## Notices

### Telemetry

Xamarin Workbooks & Inspector collects usage data and sends it to Microsoft to
help improve our products and services. [Read our privacy statement to learn
more](https://go.microsoft.com/fwlink/?LinkID=824704).

Users may opt out of telemetry and usage data collection from the Preferences
dialog.

### Third Party Code

Xamarin Workbooks & Inspector incorporates open source code from external
projects. See [ThirdPartyNotices.txt](ThirdPartyNotices.txt) for attribution.

## Contributing

This project welcomes contributions and suggestions. Most contributions require
you to agree to a Contributor License Agreement (CLA) declaring that you have
the right to, and actually do, grant us the rights to use your contribution.
For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether
you need to provide a CLA and decorate the PR appropriately (e.g., label,
comment). Simply follow the instructions provided by the bot. You will only
need to do this once across all repositories using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/)
or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any
additional questions or comments.

## DIY

### External Build Dependencies

#### macOS Dependencies

| Component                       | Version      | Download                                                                                                                                                          |
| :------------------------------ | :----------- | :---------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| NuGet CLI                       | 4.3.0        | [nuget.exe](https://dist.nuget.org/win-x86-commandline/v4.3.0/nuget.exe)                                                                                          |
| Xcode                           | 9.0.0        |                                                                                                                                                                   |
| Node.js                         | 6.11.4       | [node-v6.11.4.pkg](https://nodejs.org/dist/6.11.4/node-v6.11.4.pkg)                                                                                               |
| .NET Core                       | 2.0.2        | [dotnet-sdk-2.0.2-osx-x64.pkg](https://download.microsoft.com/download/7/3/A/73A3E4DC-F019-47D1-9951-0453676E059B/dotnet-sdk-2.0.2-osx-x64.pkg)                   |
| PowerShell Core                 | 6.0.0-beta.9 | [powershell-6.0.0-beta.9-osx.10.12-x64.pkg](https://github.com/PowerShell/PowerShell/releases/download/v6.0.0-beta.9/powershell-6.0.0-beta.9-osx.10.12-x64.pkg)   |
| Mono                            | 5.4.1.4      | [MonoFramework-MDK-5.4.1.4.macos10.xamarin.universal.pkg](https://dl.xamarin.com/MonoFrameworkMDK/Macx86/MonoFramework-MDK-5.4.1.4.macos10.xamarin.universal.pkg) |
| Xamarin.iOS                     | 11.4.0.93    | [xamarin.ios-11.4.0.93.pkg](https://dl.xamarin.com/MonoTouch/Mac/xamarin.ios-11.4.0.93.pkg)                                                                       |
| Xamarin.Mac                     | 4.0.0.93     | [xamarin.mac-4.0.0.93.pkg](https://dl.xamarin.com/XamarinforMac/Mac/xamarin.mac-4.0.0.93.pkg)                                                                     |
| Xamarin.Android                 | 8.1.0-21     | [xamarin.android-8.1.0-21.pkg](https://dl.xamarin.com/MonoforAndroid/Mac/xamarin.android-8.1.0-21.pkg)                                                            |
| Visual Studio for Mac (Preview) | 7.3.0.708    | [VisualStudioForMac-Preview-7.3.0.708.dmg](https://dl.xamarin.com/VsMac/VisualStudioForMac-Preview-7.3.0.708.dmg)                                                 |

#### Windows Dependencies

| Component | Version | Download                                                                 |
| :-------- | :------ | :----------------------------------------------------------------------- |
| NuGet CLI | 4.3.0   | [nuget.exe](https://dist.nuget.org/win-x86-commandline/v4.3.0/nuget.exe) |

### Building

Ensure git submodules are correct:

```bash
git submodule sync
git submodule update --recursive --init
```

Building is straightforward on Mac and Windows:

```bash
msbuild Build.proj
```

If you want to build a `Release` build on Windows (for example, you want to
build an installer), you will need to build in a slightly different fashion.
First, make sure that you connect to a Mac build host via Visual Studio at
least once. You can do this by doing the following:

* Open Visual Studio
* Go to _Tools → Options → Xamarin → iOS Settings_
* Click "Find Xamarin Mac Agent"
* Select a Mac on your network, or add one by name
* Enter credentials when prompted

Once the connection completes, click OK to close all the dialogs. Then,
build the `Release` configuration by running the following:

```bash
msbuild Build.proj \
  /p:MacBuildHostAddress="<hostname-or-ip-of-your-mac>" \
  /p:MacBuildHostUser="<mac-username>" \
  /p:Configuration=Release /t:Build,Install
```

This is needed because the installer build now needs a zipped copy of the
Xamarin.iOS workbook app from the server. The `Xamarin.Workbooks.iOS` project
will do the build and copy automatically when a Mac build host is used. If you
are building in Debug, you can omit those properties unless you need the
Workbook app to be copied locally, in which case, include them there as well.

**Note:** the build will read properties from `Build/Local.props` as well,
for example:

```xml
<Project xmlns="http://schemas.microsoft.com/developer/msbuild/2003">
  <PropertyGroup>
    <MacBuildHostAddress>porkbelly</MacBuildHostAddress>
    <MacBuildHostUser>aaron</MacBuildHostUser>
  </PropertyGroup>
</Project>
```

### Testing

```bash
msbuild /t:TestRegressions,TestInspectorInjector Build.proj
```

[vs]: https://www.visualstudio.com/vs/preview
[dotnetcore]: https://dotnetcli.blob.core.windows.net/dotnet/Sdk/2.0.0-preview3-006689/dotnet-sdk-2.0.0-preview3-006689-win-x64.exe
[node]: https://nodejs.org/en/download/
[7zip]: http://www.7-zip.org/download.html

[macbuildstatus]: https://devdiv.visualstudio.com/_apis/public/build/definitions/0bdbc590-a062-4c3f-b0f6-9383f67865ee/6539/badge "macOS Build Status"

[winbuildstatus]: https://devdiv.visualstudio.com/_apis/public/build/definitions/0bdbc590-a062-4c3f-b0f6-9383f67865ee/6563/badge "Windows Build Status"
