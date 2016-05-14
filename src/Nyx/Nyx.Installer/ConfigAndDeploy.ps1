#
# ConfigAndDeploy.ps1
#

# Arguments
param (
	[Parameter(Mandatory=$true)] 
	[string] 
	$ConfigurationFile
)

$currentFolder = (pwd)
$configFolder = Split-Path $ConfigurationFile
$configFile = [System.IO.Path]::GetFileName($ConfigurationFile)
$packFolder = Split-Path $script:MyInvocation.MyCommand.Path

Write-Host "Current folder is $currentFolder"
Write-Host "Config file path is $ConfigurationFile"
Write-Host "Config folder is $configFolder"
Write-Host "Pack folder is $packFolder"

Write-Host "Copying configuration..."
Copy-Item -Path "$configFolder\*" -Destination "$packFolder" -Force -Container:$false

Write-Host "Running deployment..."
pushd $packFolder
.\deploy.cmd $configFile
if ($LastExitCode -ne 0)
{
	throw "Deployment failed with error code: $LastExitCode"
}
else
{
	Write-Host "Deployment successfully completed."
}