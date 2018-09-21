param
(
	[AllowEmptyString()]
	[Parameter(Mandatory)]
	[string]$nuGetApiKey,
	[AllowEmptyString()]
	[Parameter(Mandatory)]
	[string]$repository
)

try
{
	$requiredModuleName = "PowerShellGet";
	$requiredModule = Get-Module -Name $requiredModuleName;

	if(!$requiredModule)
	{
		Install-Module -Name $requiredModuleName -Scope CurrentUser;
	}

	# If you want to register your own PowerShell-repository use the command Register-PSRepository (PowerShellGet).

	$moduleManifestFile = Get-ChildItem -Filter "*.psd1" -Path $PSScriptRoot -Recurse:$true;
	$modulePath = $moduleManifestFile.Directory.FullName;

	$parameters = New-Object System.Collections.Hashtable;

	if($nuGetApiKey)
	{
		$parameters.Add("NuGetApiKey", $nuGetApiKey);
	}

	$parameters.Add("Path", $modulePath);

	if($repository)
	{
		$parameters.Add("Repository", $repository);
	}

	$parameters.Add("Verbose", $true);

	Publish-Module @parameters;

	Write-Host "The module '$($modulePath)' was published successfully." -ForegroundColor Green;
}
catch
{
	Write-Host "Could not publish module. $($_.Exception.Message)" -ForegroundColor Red;
}

Write-Host "Press any key to exit..." -ForegroundColor Yellow -NoNewline:$true;
Read-Host;