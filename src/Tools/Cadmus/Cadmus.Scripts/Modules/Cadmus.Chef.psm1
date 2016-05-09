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
	Log-Info "Creating temp directory..."
	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $ChefInfo
	Log-Info "Temp directory: $($ChefInfo.TempDir)"

	Log-Info "Copying cookbooks..."
	$ComputerInfo.ChefCookbooks | Foreach-Object {
		Copy-Item "Cookbooks\$_\*" -Destination "$($ChefInfo.TempDir)\Cookbooks\$_" `
		          -ToSession $ComputerInfo.Session -Recurse
	}
	
	$ChefInfo.Recipes = $ComputerInfo.ChefRecipes -join ','
	if (-Not [string]::IsNullOrEmpty($ChefInfo.Recipes))
	{
		Log-Info "Starting chef-solo..."
		Invoke-Command -Session $ComputerInfo.Session -ArgumentList $ChefInfo -ScriptBlock {
			param ($ChefInfo)
			pushd  $ChefInfo.TempDir

			chef-client -z -o $ChefInfo.Recipes
			if ($LastExitCode -ne 0)
			{
				Start-Sleep 1
				throw "Chef failed: $LastExitCode"
			}

			popd
			Remove-Item -Recurse -Force $ChefInfo.TempDir
		}
	}
}