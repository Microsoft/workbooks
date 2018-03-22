# Version @BUILD_DOCUMENTATION_VERSION@

Please refer to the [detailed release notes][docs-detailed-release-notes] and
full product documentation for [Workbooks][docs-workbooks] and
[Inspector][docs-inspector] for complete information.

This is a significant update to the [1.3 series][13-series], with a number of
new features and bug fixes.

## Now Open Source

Xamarin Workbooks has now been released as open source software
under the MIT license. Ongoing development will happen in the open on
[GitHub](https://github.com/Microsoft/workbooks). We invite interested users and
developers to get involved with the project.

## New & Improved

* Support for iOS 11 and Xcode 9.

* Camera controls on the 3D view inspector have been enhanced and now share
  functionality across macOS and Windows with support for Pan, Zoom and Rotate.

* The visual tree inspector is now more consistent between Windows and macOS
  and has improved view selection and navigation features.

* The property panel in the view inspector is now based on
  [Xamarin.PropertyEditing][proppy], which provides a number of improvements:
  - Properties can now be edited on macOS.
  - Performance improvements thanks to loading properties asynchronously.
  - Editing support for enum, size, and rectangle properties.

* Line-wrapping may be turned off for code cells via the preferences dialog.

* It is now possible for integrations to asynchronously post results to
  code cells. This is the groundwork for supporting `IObservable` and allows
  for [deeper integration with cell compilations][cell-compilations].

* It is now possible to use ASP.NET Core in your .NET Core workbooks.

* Signature help now behaves more like Visual Studio.

* Choose the new `Report an Issue` menu item in the `Help` menu to easily
  report issues, and `Reveal Log File` to quickly find the latest log.

* Xamarin.Forms support has been bumped to 2.5.0.

* The New Workbook dialog now defaults to the last selected type.

## Notable Bug Fixes

* Additional accessibility fixes for High Contrast mode users on Windows,
  particularly for buttons and menus in the High Contrast White theme.

* The plain text formatter for strings now preserves whitespace in formatted
  output.

* Fix generic type name rendering.

* Fix rendering of emoji in C# string literals.

* Workbooks are now marked as dirty when cells are deleted, preventing possible
  stale workbook files on disk.

* Fixed a few minor issues with NuGet package restoration.

* Fixed a rendering issue in the Mac sidebar. Thank you to Yusuke Yamada
  (@yamachu) for our
  [first ever public contribution](https://github.com/Microsoft/workbooks/pull/97)!

* Fix user interface rendering issues after unlocking your computer on Windows.

* Fixed an SDK location bug that prevented Android workbooks from running.

* Add `workbook` tool to `PATH` using `/etc/paths.d` instead of writing a
  script to `/usr/local/bin`.

* Fix custom attributes using types defined in the workbook, enabling
  custom JSON deserialization.

## Other Notable Changes

[See the full documentation for details][docs-workbooks-logs]
on the following changes:

* On macOS, `/Applications/Xamarin Workbooks.app` now has a bundle identifier
  of `com.xamarin.Workbooks` instead of `com.xamarin.Inspector`.

* The log file directory has changed to facilitate more fully splitting
  Inspector and Workbooks into separate distributables.

## Known Issues

* NuGet Limitations
  - Native libraries are supported only on iOS, and only when linked with
    the managed library.
  - Packages which depend on `.targets` files or PowerShell scripts will likely
    fail to work as expected.
  - To modify a package dependency, edit the workbook's manifest with
    a text editor. A more complete package management UI is on the way.

[github]: https://github.com/Microsoft/workbooks
[proppy]: https://github.com/xamarin/Xamarin.PropertyEditing
[cell-compilations]: https://github.com/Microsoft/workbooks/blob/master/Samples/CompilationIntegration/AgentIntegration.cs

[docs-workbooks]: https://developer.xamarin.com/guides/cross-platform/workbooks/
[docs-inspector]: https://developer.xamarin.com/guides/cross-platform/inspector/
[docs-detailed-release-notes]: https://developer.xamarin.com/releases/interactive/interactive-1.4/
[docs-workbooks-logs]: https://developer.xamarin.com/guides/cross-platform/workbooks/install/#Log_Files
[13-series]: https://developer.xamarin.com/releases/interactive/interactive-1.3
