#
# Cadmus.Chef.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -DisableNameChecking

function Deploy-Chef
{
	param ($ComputerInfo, $ChefInfo)
	if (-Not $ChefInfo.Deploy -or -Not $ComputerInfo.ConfigChef) { return }

	Log-Info "Applying Chef configuration to $($ComputerInfo.ConfigName)..."

	Ensure-RemotingSession $ComputerInfo
	Log-Info "Copy files..."
	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $ChefInfo
	Log-Info "Temp directory: $($ChefInfo.TempDir)"
	
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $ChefInfo -ScriptBlock {
		param ($ChefInfo)
		chef-solo -v
		Remove-Item -Recurse -Force $ChefInfo.TempDir
	}
}