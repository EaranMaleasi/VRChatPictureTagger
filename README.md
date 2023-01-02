# VRChatPictureTagger

The VRChat Picture Tagger is a small application aiming to provide the answer to the following question: **Who is on that screenshot?**

It tries to give possible answers by looking through a **VRCX** database and match the time of the screenshot, with the people that were in the instance the picture was taken in. 

## Planned Features:  
- Auto-Tag pictures with people that were in the instance the moment the picture was taken
- Auto-Tag pictures with the world that it was taken in.
- Tag editor to remove/add tags form pictures and to give tags custom colors.
- Search for pictures according to people or world tags

### Possible Features after 1.0.0 is done:
- Scrubable timeline of instances and people in those instances
- auto-filter people tags on a screenshot using image recognition based on machine learning
- installer/updater

## Disclaimer  
This project is mainly a way for me to get into things like:
- [.NET 6](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6).   
- [.NET Hosting](https://learn.microsoft.com/en-us/dotnet/core/extensions/generic-host)
- [Serilog](https://serilog.net/)
- [Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)
- [Loose coupling](https://stackoverflow.com/a/37993102)
- [MVVM](https://learn.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/mvvm)
- [XAML](https://learn.microsoft.com/en-us/dotnet/desktop/wpf/xaml/?view=netdesktop-6.0)
- [WinUI 3](https://learn.microsoft.com/en-us/windows/apps/winui/winui3/create-your-first-winui3-app)

While I have already worked with most of these patterns, libraries and frameworks in my 10+ years as professional C# dev, it was mainly in an extending fashion, only adding to existing projects where architecture had already been set. This is a fresh start, a way for me to learn what works, and what doesn't, for me to fiddle and tinker around with stuff without the time pressure of a client waiting for things to be done.

## Requirements
- **Windows 10** 1809 or newer. **Windows 11**
- **.NET 6 Runtime** needs to be installed for this to run.
  - Download for [32-Bit (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.12-windows-x64-installer) 
  - Download for [64-Bit (x64)]()
- **VRCX** has to be installed 
  - a `VRCX.sqlite3` database needs to be at `%APPDATA%\VRCX\VRCX.sqlite3`).