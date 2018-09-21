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

	if(!$repository)
	{
		$repository = "PSGallery";
	}

	$source = (Get-PSRepository -Name $repository).PublishLocation;

	$nuGet = "$($PSScriptRoot)\NuGet.exe";

	Invoke-WebRequest -OutFile $nuGet -Uri "https://nuget.org/nuget.exe";

	$packageName = (Get-ChildItem -Filter "*.nupkg" -Path $PSScriptRoot).Name;

	if(!$nuGetApiKey)
	{
		&$nuGet push $packageName -Source $source;
	}
	else
	{
		&$nuGet push $packageName -ApiKey $nuGetApiKey -Source $source;
	}

	Write-Host "The module-package '$($packageName)' was published successfully." -ForegroundColor Green;
}
catch
{
	Write-Host "Could not publish module. $($_.Exception.Message)" -ForegroundColor Red;
}

Write-Host "Press any key to exit..." -ForegroundColor Yellow -NoNewline:$true;
Read-Host;