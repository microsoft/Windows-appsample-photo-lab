---
page_type: sample
languages:
- csharp
products:
- windows
- windows-uwp
statusNotificationTargets:
- codefirst@microsoft.com
---

<!---
  category: ControlsLayoutAndText FilesFoldersAndLibraries
-->

# PhotoLab sample

A mini-app for viewing and editing image files, demonstrating XAML layout, data binding, and UI customization features for Universal Windows Platform (UWP) apps.

> Note - This sample is targeted and tested for Windows 10, version 2004 (10.0; Build 19041), and Visual Studio 2019. If you prefer, you can use project properties to retarget the project(s) to Windows 10, version 1903 (10.0; Build 18362).

![PhotoLab sample showing the image collection page](Screenshots/PhotoLab-collection-page.png)
![PhotoLab sample showing the image editing page](Screenshots/PhotoLab-editing-page.png)

This repo includes the complete sample pictured above, plus separate versions that serve as starting points for a series of
[XAML basics tutorials](xaml-basics-starting-points).
Each of these starting points is a simplified version of the complete sample, making the code easier to browse around in as you go through each tutorial.

> **Note:** The tutorials do not proceed sequentially to build up to the complete sample, so be sure to start each tutorial by opening the correct starting point project.
Also, be sure to check out the complete sample to see additional features such as custom animations.

## Features

PhotoLab demonstrates:

* XAML layout ranging from basics to adaptive and tailored layouts.
* XAML data binding including the [{x:Bind} markup extension](https://docs.microsoft.com/windows/uwp/xaml-platform/x-bind-markup-extension).
* XAML styling and UI customization.
* Image effects from [Windows.UI.Composition](https://docs.microsoft.com/uwp/api/windows.ui.composition).
* Use of the [Windows UI Library (WinUI)](https://docs.microsoft.com/windows/apps/winui) and the [Windows Community Toolkit](https://docs.microsoft.com/windows/communitytoolkit/) (for [ReorderGridAnimation](https://docs.microsoft.com/windows/communitytoolkit/animations/reordergrid)).
* Loading images from the Pictures library using data virtualization to increase performance when there are numerous files.

## Code at a glance

If you're just interested in code snippets for certain areas and don't want to browse or run the full sample, 
check out the following files for examples of some highlighted features:

* Layout: see [MainPage.xaml](PhotoLab/MainPage.xaml#25) and [DetailPage.xaml](PhotoLab/DetailPage.xaml#25).
* Data binding with x:Bind: see [ImageGridView_DefaultItemTemplate](PhotoLab/MainPage.xaml#72) in [MainPage.xaml](PhotoLab/MainPage.xaml#25)
* Styling and customization: see [FancySliderControlTemplate](PhotoLab/DetailPage.xaml#61) in [DetailPage.xaml](PhotoLabl/DetailPage.xaml#25). 
* Image effects: see code starting with [InitializeEffects](PhotoLab/DetailPage.xaml.cs#185) in [DetailPage.xaml.cs](PhotoLab/DetailPage.xaml.cs#25).

## Related documentation and samples

* [Controls and patterns for UWP apps](https://docs.microsoft.com/windows/uwp/controls-and-patterns/index)
* [Layout for UWP apps](https://docs.microsoft.com/windows/uwp/layout/)
* [Data binding in depth](https://docs.microsoft.com/windows/uwp/data-binding/data-binding-in-depth)
* [UWP style guide](https://docs.microsoft.com/windows/uwp/style/)
* [Visual layer](https://docs.microsoft.com/windows/uwp/composition/visual-layer)
* [ListView and GridView data virtualization](https://docs.microsoft.com/windows/uwp/debug-test-perf/listview-and-gridview-data-optimization)
* [Data virtualization sample](https://github.com/Microsoft/Windows-universal-samples/tree/master/Samples/XamlDataVirtualization)

## External libraries used in this sample

* [Windows UI Library (WinUI)](https://docs.microsoft.com/windows/apps/winui)
* [Windows Community Toolkit](https://docs.microsoft.com/windows/communitytoolkit/)

## Universal Windows Platform development

### Prerequisites

- Windows 10. Minimum: Windows 10, version 1809 (10.0; Build 17763), also known as the Windows 10 October 2018 Update.
- [Windows 10 SDK](https://developer.microsoft.com/windows/downloads/windows-10-sdk). Minimum: Windows SDK version 10.0.17763.0 (Windows 10, version 1809).
- [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) (or Visual Studio 2017). You can use the free Visual Studio Community Edition to build and run Windows Universal Platform (UWP) apps.

To get the latest updates to Windows and the development tools, and to help shape their development, join 
the [Windows Insider Program](https://insider.windows.com).

## Running the sample

The default project is PhotoLab and you can Start Debugging (F5) or Start Without Debugging (Ctrl+F5) to try it out. 
The app will run in the emulator or on physical devices. 

> **Note:** The platform target currently defaults to ARM, so be sure to change that to x64 or x86 if you want to test on a non-ARM device. 


