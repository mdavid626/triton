#
# Cadmus.Msi.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -DisableNameChecking

function Deploy-Msi 
{
	param ($ComputerInfo, $MsiInfo)
	if (-Not $MsiInfo.Deploy) { return }
	Log-Info 'Deploying MSI...'
	Start-Verbose
	foreach ($computerInfo in $ComputerInfo)
	{
		Ensure-RemotingSession $computerInfo
		Ensure-RemoteTempDirectory $computerInfo.Session $MsiInfo

		Log-Info "Temp directory: $($MsiInfo.TempDir)"
		Copy-Item "setup.msi" -Destination $MsiInfo.TempDir -ToSession $computerInfo.Session
		Copy-Item "cab1.cab" -Destination $MsiInfo.TempDir -ToSession $computerInfo.Session

		Invoke-Command -Session $computerInfo.Session -ArgumentList $MsiInfo -ScriptBlock {
			param ($MsiInfo)
			pushd $MsiInfo.TempDir

			$args = @('/i', "setup.msi", '/qn', '/passive')   
			$proc = Start-Process msiexec -NoNewWindow -Wait -ArgumentList $args -PassThru -ErrorAction Stop -WorkingDirectory $MsiInfo.TempDir
			if ($proc.ExitCode -ne 0) {
				throw "MSI installation failed: $($proc.ExitCode)"   
			}

			popd
			Remove-Item -Recurse -Force $MsiInfo.TempDir
		}
	}
	Stop-Verbose
}