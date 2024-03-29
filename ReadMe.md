# PowerShell-Transforming

PowerShell commands for json-, package- and xml-transformation. This is, from version 2.0.0, a portable module that can run on Linux, Mac-OS and Windows. This library is simply a PowerShell layer for [**.NET-Transforming**](https://github.com/RegionOrebroLan/.NET-Transforming/).

[![PowerShell Gallery](https://img.shields.io/powershellgallery/v/RegionOrebroLan.Transforming.svg?label=PowerShell%20Gallery)](https://www.powershellgallery.com/packages/RegionOrebroLan.Transforming/)

## Commands

### File-transforming

#### JSON-transform

    New-FileTransform `
        -Destination "C:\Data\Transforms\Out\AppSettings.json" `
        -Source "C:\Data\Transforms\In\AppSettings.json" `
        -Transformation "C:\Data\Transforms\In\AppSettings.Transformation.json";

#### XML-transform

    New-FileTransform `
        -Destination "C:\Data\Transforms\Out\Web.config" `
        -Source "C:\Data\Transforms\In\Web.config" `
        -Transformation "C:\Data\Transforms\In\Web.Transformation.config";

### Package-transforming

#### Patterns

For handling patterns the Microsoft.Extensions.FileSystemGlobbing.Matcher class is used under the hood.

- FileToTransformPatterns
- PathToDeletePatterns

Examples: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-5.0#remarks

NuGet: https://www.nuget.org/packages/Microsoft.Extensions.FileSystemGlobbing

Patterns with absolute paths does not result in any matches.

#### Directory to directory transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-To-Delete/**/*", "**/File-To-Delete.*" `
        -Source "C:\Data\Transforms\In\Package" `
        -TransformationNames "Release", "Test";

#### Directory to zip-file transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package.zip" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-To-Delete/**", "**/File-To-Delete.*" `
        -Source "C:\Data\Transforms\In\Package" `
        -TransformationNames "Release", "Test";

#### Zip-file to zip-file transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package.zip" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-To-Delete/*", "**/File-To-Delete.*" `
        -Source "C:\Data\Transforms\In\Package.zip" `
        -TransformationNames "Release", "Test";

#### Zip-file to directory transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-To-Delete/**/*", "**/File-To-Delete.*" `
        -Source "C:\Data\Transforms\In\Package.zip" `
        -TransformationNames "Release", "Test";

## Deployment/installation

If you want to set up a local PowerShell-Gallery to test with:

    Register-PSRepository -Name "Local" -InstallationPolicy Trusted -SourceLocation "C:\Data\My-Local-PowerShell-Gallery\";

1. Download this repository and build.
2. Run **Publish-Module.ps1** in the output-directory (bin\Release).
3. Enter the NuGetApiKey if required and the name of the Repository or leave it blank to publish to "PSGallery". If you are testing with your local one, press enter for the NuGetApiKey parameter and enter "Local" for the Repository parameter.

Then you can try to install the module:

    Install-Module "RegionOrebroLan.Transforming";

or save it:

    Save-Module -Name "RegionOrebroLan.Transforming" -Path "C:\Data\Saved-PowerShell-Modules";

## Information

- [How to Package and Distribute PowerShell Cmdlets, Functions, and Scripts](http://get-powershell.com/post/2011/04/04/How-to-Package-and-Distribute-PowerShell-Cmdlets-Functions-and-Scripts.aspx)
- [Using C# to Create PowerShell Cmdlets: The Basics](https://www.red-gate.com/simple-talk/dotnet/net-development/using-c-to-create-powershell-cmdlets-the-basics/)
- [PowerShell: how to unit test your cmdlet](https://weblogs.asp.net/cazzu/PowerShellUnitTestCmdlet/)
- [Publishing C# cmdlets to the PowerShell Gallery](http://mmaitre314.github.io/2016/03/22/publishing-csharp-cmdlets-to-the-powershell-gallery.html)
- [Google: writing a powershell cmdlet](https://www.google.com/search?q=writing+a+powershell+cmdlet)
- [Google: powershell cmdlet unit test](https://www.google.com/search?q=powershell+cmdlet+unit+test)
- [Google: publish-module dll cmdlet](https://www.google.com/search?q=publish-module+dll+cmdlet)

### Portable modules

- [Portable Modules](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/writing-portable-modules)
- [Create and test PowerShell Core cmdlets in C#](https://blog.danskingdom.com/Create-and-test-PowerShell-Core-cmdlets-in-CSharp/)