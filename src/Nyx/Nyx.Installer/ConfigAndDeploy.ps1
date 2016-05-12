#
# ConfigAndDeploy.ps1
#

# Arguments
param (
	[Parameter(Mandatory=$true)] 
	[string] 
	$ConfigurationFile
)

$configFolder = Split-Path $ConfigurationFile
$configFile = [System.IO.Path]::GetFileName($ConfigurationFile)
$packFolder = Split-Path $script:MyInvocation.MyCommand.Path

Write-Host "Config file path is $ConfigurationFile"
Write-Host "Config folder is $configFolder"
Write-Host "Pack folder is $packFolder"

Write-Host "Copying configuration..."
Copy-Item -Filter * -Path $configFolder -Destination $packFolder -Force -Recurse

Write-Host "Running deployment..."
.\deploy.cmd $configFile
if ($LastExitCode -ne 0)
{
	throw "Deployment failed with error code: $LastExitCode"
}
else
{
	Write-Host "Deployment successfully completed."
}