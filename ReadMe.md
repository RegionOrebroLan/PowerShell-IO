# PowerShell-IO

PowerShell commands for IO. This library is simply a PowerShell layer for [**.NET-IO-Extensions**](https://github.com/RegionOrebroLan/.NET-IO-Extensions/).

[![PowerShell Gallery](https://img.shields.io/powershellgallery/v/RegionOrebroLan.IO.svg?label=PowerShell%20Gallery)](https://www.powershellgallery.com/packages/RegionOrebroLan.IO/)

## Commands

    Get-FileSystemEntryPathMatch "C:\Data\**\*.txt"; # The include-parameter.

    Get-FileSystemEntryPathMatch `
        -DirectoryPath "C:\Data\" `
        -Exclude "Folder\First.txt", "C:\Data\Folder\Second.txt" `
        -Include "**\*.txt";

    Get-FileSystemEntryPathMatch `
        -Exclude "Folder\First.txt", "C:\Data\Folder\Second.txt" `
        -Include "C:\Data\**\*.txt";

## Deployment/installation

If you want to set up a local PowerShell-Gallery to test with:

    Register-PSRepository -Name "Local" -InstallationPolicy Trusted -SourceLocation "C:\Data\My-Local-PowerShell-Gallery\";

1. Download this repository and build.
2. Run **Publish-Module.ps1** in the output-directory (bin\Release).
3. Enter the NuGetApiKey if required and the name of the Repository or leave it blank to publish to "PSGallery". If you are testing with your local one, press enter for the NuGetApiKey parameter and enter "Local" for the Repository parameter.

Then you can try to install the module:

    Install-Module "RegionOrebroLan.IO";

or save it:

    Save-Module -Name "RegionOrebroLan.IO" -Path "C:\Data\Saved-PowerShell-Modules";