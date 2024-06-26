# PowerShell-Transforming

PowerShell commands for json-, package- and xml-transformation. This is, from version 2.0.0, a portable module that can run on Linux, Mac-OS and Windows. This library is simply a PowerShell layer for [**.NET-Transforming**](https://github.com/RegionOrebroLan/.NET-Transforming/).

[![PowerShell Gallery](https://img.shields.io/powershellgallery/v/RegionOrebroLan.Transforming.svg?label=PowerShell%20Gallery)](https://www.powershellgallery.com/packages/RegionOrebroLan.Transforming/)

## 1 Commands

### 1.1 File-transforming

#### 1.1.1 JSON-transform

    New-FileTransform `
        -Destination "C:\Data\Transforms\Out\appsettings.json" `
        -Source "C:\Data\Transforms\In\appsettings.json" `
        -Transformation "C:\Data\Transforms\In\appsettings.Transformation.json";

#### 1.1.2 XML-transform

    New-FileTransform `
        -Destination "C:\Data\Transforms\Out\Web.config" `
        -Source "C:\Data\Transforms\In\Web.config" `
        -Transformation "C:\Data\Transforms\In\Web.Transformation.config";

### 1.2 Package-transforming

#### 1.2.1 Patterns

For handling patterns the Microsoft.Extensions.FileSystemGlobbing.Matcher class is used under the hood.

- FileToTransformPatterns
- PathToDeletePatterns

Examples: https://docs.microsoft.com/en-us/dotnet/api/microsoft.extensions.filesystemglobbing.matcher?view=dotnet-plat-ext-5.0#remarks

NuGet: https://www.nuget.org/packages/Microsoft.Extensions.FileSystemGlobbing

Patterns with absolute paths does not result in any matches.

#### 1.2.2 Directory to directory transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-to-delete/**/*", "**/File-to-delete.*" `
        -Source "C:\Data\Transforms\In\Package" `
        -TransformationNames "Release", "Test";

#### 1.2.3 Directory to zip-file transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package.zip" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-to-delete/**", "**/File-to-delete.*" `
        -Source "C:\Data\Transforms\In\Package" `
        -TransformationNames "Release", "Test";

#### 1.2.4 Zip-file to zip-file transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package.zip" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-to-delete/*", "**/File-to-delete.*" `
        -Source "C:\Data\Transforms\In\Package.zip" `
        -TransformationNames "Release", "Test";

#### 1.2.5 Zip-file to directory transform

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-to-delete/**/*", "**/File-to-delete.*" `
        -Source "C:\Data\Transforms\In\Package.zip" `
        -TransformationNames "Release", "Test";

#### 1.2.6 Cleanup parameter

The actual transforming is done under the %temp%-directory. The "Cleanup" parameter (defaults to true) is for removing the temporary transform-directories or not.

    New-PackageTransform `
        -Cleanup false `
        -Destination "C:\Data\Transforms\Out\Package" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-to-delete/**/*", "**/File-to-delete.*" `
        -Source "C:\Data\Transforms\In\Package" `
        -TransformationNames "Release", "Test";

### 1.3 Common

All the commands above also have the parameter **-AvoidByteOrderMark true/false**. The default value for this parameter is:

- On Windows: false
- On other platforms (Linux/MacOS): true

This parameter controls the result of the transformation. If a source file that will be transformed has a BOM (Byte Order Mark) the destination file should also have a BOM. This is not always desired, e.g. on a Linux system. So if the source file has a BOM but the parameter "AvoidByteOrderMark" is set to true, the destination file will not have a BOM.

### 1.4 Logging

This PowerShell-module uses [**.NET-Transforming**](https://github.com/RegionOrebroLan/.NET-Transforming/) that may log information. A [**CommandLogger**](/Source/Project/Logging/CommandLogger.cs#L50) is used to write the logs to the PowerShell-console.

- **LogLevel.Critical** & **LogLevel.Error** write errors (Cmdlet.WriteError).
- **LogLevel.Debug** write debug (Cmdlet.WriteDebug).
- **LogLevel.Information** write information (Cmdlet.WriteInformation).
- **LogLevel.Trace** write verbose (Cmdlet.WriteVerbose).
- **LogLevel.Warning** write warnings (Cmdlet.WriteWarning).

To write debug logs to the console you need to add the **-Debug** parameter:

    New-FileTransform `
        -Destination "C:\Data\Transforms\Out\appsettings.json" `
        -Source "C:\Data\Transforms\In\appsettings.json" `
        -Transformation "C:\Data\Transforms\In\appsettings.Transformation.json" `
        -Debug;

To write verbose (trace) logs to the console you need to add the **-Verbose** parameter:

    New-PackageTransform `
        -Destination "C:\Data\Transforms\Out\Package" `
        -FileToTransformPatterns "**/*.config", "**/*.json", "**/*.xml" `
        -PathToDeletePatterns "**/Directory-to-delete/**/*", "**/File-to-delete.*" `
        -Source "C:\Data\Transforms\In\Package" `
        -TransformationNames "Release", "Test" `
        -Verbose;

**Note: At the moment [**.NET-Transforming**](https://github.com/RegionOrebroLan/.NET-Transforming/) don't write any logs. The logging functionality is recently implemented.**

## 2 Development

### 2.1 Signing

Drop the "StrongName.snk" file in the repository-root. The file should not be included in source control.

### 2.2 Build warnings

At the moment we get the following warning (PowerShell-7-tests):

- [NETSDK1206](https://learn.microsoft.com/en-us/dotnet/core/tools/sdk-errors/netsdk1206)

Haven't investigated it further.

## 3 Tests

The tests in this solution are a mix of integration- and unit-tests together.

### 3.1 Links

- Google: [icommandruntime powershell testing](https://www.google.com/search?q=icommandruntime+powershell+testing)
- [Unit Testing Powershell Cmdlets in C#](https://fgimian.github.io/unit-testing-powershell-cmdlets-in-c-sharp/)
- [Testing PowerShell cmdlets written in C#](https://pawelszczygielski.pl/2021/05/06/testing-powershell-cmdlets-written-in-c)
- [PowerShell Cmdlet tests](https://github.com/deadlydog/PowerShellCmdletInCSharpExample)
- [PowerShell testing in C#](https://gist.github.com/DigitalAXPP/935255e8ec984f3e24245386f491bcf1)

## 4 Deployment/installation

### 4.1 PowerShell-Gallery

If you want to set up a local PowerShell-Gallery to test with:

    Register-PSRepository -Name "PowerShell-Transforming" -InstallationPolicy Trusted -SourceLocation "{SOLUTION-DIRECTORY}\.powershell-repository";

or run the following script:

- [Register.ps1](/.powershell-repository/Register.ps1)

If you want to remove it:

	Unregister-PSRepository -Name "PowerShell-Transforming";

or run the following script:

- [Unregister.ps1](/.powershell-repository/Unregister.ps1)

Get all module repositories:

	Get-PSRepository

Get module repositories by name (with wildcard):

	Get-PSRepository -Name "*Something*"

More information:

- [PowerShellGet](https://learn.microsoft.com/en-us/powershell/module/powershellget#powershellget)

### 4.2 Install

1. Download this repository and build.
2. Run **Publish-Module.ps1** in the output-directory (bin\Release).
3. Enter the NuGetApiKey if required and the name of the Repository or leave it blank to publish to "PSGallery". If you are testing with your local one, press enter for the NuGetApiKey parameter and enter "PowerShell-Transforming" for the Repository parameter.

Then you can try to install the module:

    Install-Module "RegionOrebroLan.Transforming";

or (local)

	Install-Module "RegionOrebroLan.Transforming" -Repository "PowerShell-Transforming";

or save it:

    Save-Module -Name "RegionOrebroLan.Transforming" -Path "C:\Data\Saved-PowerShell-Modules";

To uninstall the module:

    Uninstall-Module "RegionOrebroLan.Transforming";

#### 4.2.1 Example

Install on a Linux build-agent via cmd.exe on Windows. The scenario is that you have version 2.0.0 installed and you want to install version 2.1.0. You also want to uninstall version 2.0.0 before installing version 2.1.0.

1. Open cmd.exe
2. `ssh user@build-agent`
3. `sudo pwsh`
4. `(Get-InstalledModule RegionOrebroLan.Transforming).InstalledLocation` - should give "/usr/local/share/powershell/Modules/RegionOrebroLan.Transforming/2.0.0" if it was installed with scope "AllUsers"
5. `Uninstall-Module RegionOrebroLan.Transforming` - if you want to remove previous versions
6. `Install-Module RegionOrebroLan.Transforming -Scope "AllUsers"`
7. `(Get-InstalledModule RegionOrebroLan.Transforming).InstalledLocation` - should give "/usr/local/share/powershell/Modules/RegionOrebroLan.Transforming/2.1.0"

### 4.3 Other

To see if anything is installed from your local PowerShell-repository:

	Find-Module -Repository "PowerShell-Transforming"

The files in [.powershell-repository](/.powershell-repository):

- [ReadMe.md](/.powershell-repository/ReadMe.md)
- [Register.ps1](/.powershell-repository/Register.ps1)
- [Unregister.ps1](/.powershell-repository/Unregister.ps1)

give warnings but it seem to work anyhow.

## 5 Information

- [How to Package and Distribute PowerShell Cmdlets, Functions, and Scripts](http://get-powershell.com/post/2011/04/04/How-to-Package-and-Distribute-PowerShell-Cmdlets-Functions-and-Scripts.aspx)
- [Using C# to Create PowerShell Cmdlets: The Basics](https://www.red-gate.com/simple-talk/dotnet/net-development/using-c-to-create-powershell-cmdlets-the-basics/)
- [PowerShell: how to unit test your cmdlet](https://weblogs.asp.net/cazzu/PowerShellUnitTestCmdlet/)
- [Publishing C# cmdlets to the PowerShell Gallery](http://mmaitre314.github.io/2016/03/22/publishing-csharp-cmdlets-to-the-powershell-gallery.html)
- [Google: writing a powershell cmdlet](https://www.google.com/search?q=writing+a+powershell+cmdlet)
- [Google: powershell cmdlet unit test](https://www.google.com/search?q=powershell+cmdlet+unit+test)
- [Google: publish-module dll cmdlet](https://www.google.com/search?q=publish-module+dll+cmdlet)

### 5.1 Portable modules

- [Portable Modules](https://docs.microsoft.com/en-us/powershell/scripting/dev-cross-plat/writing-portable-modules)
- [Create and test PowerShell Core cmdlets in C#](https://blog.danskingdom.com/Create-and-test-PowerShell-Core-cmdlets-in-CSharp/)