#
# DownloadAndDeploy.ps1
#

param ($Version, $ConfigurationFile)

$script:ErrorActionPreference = 'Stop'

Write-Host "Downloading package..."
wget "https://www.myget.org/F/cymric/api/v2/package/Nyx.Installer/$Version" -outfile package.zip

Write-Host "Unpacking package..."
Add-Type -AssemblyName System.IO.Compression.FileSystem
[System.IO.Compression.ZipFile]::ExtractToDirectory('package.zip', 'package')
rm package.zip -force

Write-Host "Downloading configuration..."
wget $ConfigurationFile -outfile "package/pack/config.xml"

Write-Host "Running deployment..."
pushd package\pack
#.\deploy.cmd
cmd /c echo test
popd
if ($LastExitCode -ne 0) 
{ 
	throw "Deployment failed with error code: $LastExitCode"
}
else
{
	Write-Host "Deployment successfully completed."
}