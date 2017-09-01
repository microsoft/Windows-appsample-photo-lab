<!---
  category: ControlsLayoutAndText FilesFoldersAndLibraries
-->

# PhotoLab sample

A mini-app for viewing and editing image files, demonstrating XAML layout, data binding, and UI customization features for Universal Windows Platform (UWP) apps.

![PhotoLab sample showing the image collection page](Screenshots/PhotoLab-collection-page.png)
![PhotoLab sample showing the image editing page](Screenshots/PhotoLab-editing-page.png)

This repo includes the complete sample pictured above, plus separate versions that serve as starting points for a series of 
[XAML basics tutorials](xaml-basics-starting-points). 
Each of these starting points is a simplified version of the complete sample, making the code easier to browse around in as you go through each tutorial.

> **Note:** The tutorials do not proceed sequentially to build up to the complete sample, so be sure to start each tutorial by opening the correct starting point project. 
Also, be sure to check out the complete sample to see additional features such as custom animations and phone support. 

## Features

PhotoLab demonstrates:
	
* XAML layout ranging from basics to adaptive and tailored layouts. 
* XAML data binding including the [{x:Bind} markup extension](https://docs.microsoft.com/windows/uwp/xaml-platform/x-bind-markup-extension).
* XAML styling and UI customization.
* Image effects from [Windows.UI.Composition](https://docs.microsoft.com/en-us/uwp/api/windows.ui.composition).
* The use of open source libraries including the [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit) (for [ReorderGridAnimation](http://docs.uwpcommunitytoolkit.com/en/master/animations/ReorderGrid/) and [Telerik UI for UWP](https://github.com/telerik/UI-For-UWP) (for [RadRating control](http://docs.telerik.com/devtools/universal-windows-platform/controls/radrating/rating-gettingstarted)).

## Code at a glance

If you're just interested in code snippets for certain areas and don't want to browse or run the full sample, 
check out the following files for examples of some highlighted features:

* Layout: see [MainPage.xaml](PhotoLab/MainPage.xaml#25) and [DetailPage.xaml](PhotoLab/DetailPage.xaml#25).
* Data binding with x:Bind: see [ImageGridView_DefaultItemTemplate](PhotoLab/MainPage.xaml#72) in [MainPage.xaml](PhotoLab/MainPage.xaml#25)
* Styling and customization: see [FancySliderControlTemplate](PhotoLab/DetailPage.xaml#61) in [DetailPage.xaml](PhotoLabl/DetailPage.xaml#25). 
* Image effects: see code starting with [InitializeEffects](PhotoLab/DetailPage.xaml.cs#185) in [DetailPage.xaml.cs](PhotoLab/DetailPage.xaml.cs#25).

## Related documentation

* [Controls and patterns for UWP apps](https://docs.microsoft.com/windows/uwp/controls-and-patterns/index)
* [Layout for UWP apps](https://docs.microsoft.com/en-us/windows/uwp/layout/)
* [Data binding in depth](https://docs.microsoft.com/windows/uwp/data-binding/data-binding-in-depth)
* [UWP style guide](https://docs.microsoft.com/en-us/windows/uwp/style/)
* [Visual layer](https://docs.microsoft.com/en-us/windows/uwp/composition/visual-layer)

## External libraries used in this sample

* [UWP Community Toolkit](https://github.com/Microsoft/UWPCommunityToolkit)
* [Telerik UI for UWP](https://github.com/telerik/UI-For-UWP)

## Universal Windows Platform development

This sample requires Visual Studio 2017 and the Windows Software Development Kit (SDK) for Windows 10. 

[Get a free copy of Visual Studio 2017 Community Edition with support for building Universal Windows apps](http://go.microsoft.com/fwlink/?LinkID=280676)

Additionally, to be informed of the latest updates to Windows and the development tools, join the 
[Windows Insider Program](https://insider.windows.com/ "Become a Windows Insider").

## Running the sample

The default project is PhotoLab and you can Start Debugging (F5) or Start Without Debugging (Ctrl+F5) to try it out. 
The app will run in the emulator or on physical devices. 

> **Note:** The platform target currently defaults to ARM, so be sure to change that to x64 or x86 if you want to test on a non-ARM device. 
