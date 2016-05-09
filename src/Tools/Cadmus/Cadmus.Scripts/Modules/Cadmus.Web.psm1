#
# Cadmus.Web.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -DisableNameChecking

function Parametrize-SetParametersFile()
{
	param ($File, $WebInfo)
	Replace-XmlValue $File "/parameters/setParameter[@name='IIS Web Application Name']/@value" $WebInfo.AppPath
	Replace-XmlValue $File "/parameters/setParameter[@name='AuthMode']/@value" $WebInfo.AppAuthMode
	Replace-XmlValue $File "/parameters/setParameter[@name='MachineValidationKey']/@value" $WebInfo.MachineValidationKey
	Replace-XmlValue $File "/parameters/setParameter[@name='MachineDecryptionKey']/@value" $WebInfo.MachineDecryptionKey
	Replace-XmlValue $File "/parameters/setParameter[@name='DefaultConnectionString']/@value" $WebInfo.ConnectionString
}

function Deploy-WebApp()
{
	param ($ComputerInfo, $WebInfo)
	if (-Not $WebInfo.Deploy) { return }

	Log-Info "Deploying WebApp $($WebInfo.AppName)..."
	Ensure-RemotingSession $ComputerInfo
	Start-Verbose

	# Parameterize SetParameters.xml
	$file = [System.IO.Path]::Combine($WebInfo.PackageFolder, $WebInfo.PackageParamsXml)
	Log-Info "Parametrizing $file"
	Parametrize-SetParametersFile -File $file -WebInfo $WebInfo

	# Copy package
	Log-Info "Copy package files to app server folder"
	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $WebInfo
	Log-Info "Temp directory: $($WebInfo.TempDir)"
	Copy-Item "$($WebInfo.PackageFolder)\*" -Destination $WebInfo.TempDir -ToSession $ComputerInfo.Session

	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $WebInfo -ScriptBlock {
		param ($Web)
		Import-Module WebAdministration

		# Creating WebApplication
		if (-Not (Test-Path $Web.AppPhysicalPath)) 
		{
			New-Item $Web.AppPhysicalPath -ItemType Directory
		}

		New-WebApplication -Name $Web.AppName -Site $Web.SiteName -PhysicalPath $Web.AppPhysicalPath -ApplicationPool $Web.AppPoolName -Force

		# Running WebDeploy
		$run = [System.IO.Path]::Combine($Web.TempDir, $Web.PackageDeployCmd)
		&$run /y
		if ($LastExitCode -ne 0) { throw "WebDeploy failed: $LastExitCode" }

		Remove-Item -Recurse -Force $Web.TempDir
	}
	Stop-Verbose
}

function Deploy-WebSite()
{
	param ($ComputerInfo, $SiteInfo)
	if (-Not $SiteInfo.Deploy) { return }
	Log-Info "Deploying WebSite $($SiteInfo.Name)..."

	Ensure-RemotingSession $ComputerInfo
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $SiteInfo -ScriptBlock {
		param ($SiteInfo)
		Import-Module WebAdministration

		if ($SiteInfo.RemoveDefault -and (Test-Path 'IIS:\Sites\Default Web Site'))
		{
			Remove-Website -Name 'Default Web Site'
		}

		if ($SiteInfo.AppPoolDeploy)
		{
			$address = "IIS:\AppPools\$($SiteInfo.AppPoolName)"
			if (-Not (Test-Path $address))
			{
				Write-Host "Creating new app pool..."
				New-WebAppPool -Name $SiteInfo.AppPoolName -Force
			}
			
			Write-Host "Setting app pool properties..."
			$identity = @{ 
				UserName = $SiteInfo.AppPoolUsername; 
				Password = "heslo"; 
				IdentityType = $SiteInfo.AppPoolIdentity;
			}
			Set-ItemProperty $address processModel $identity
		}
		
		$address = "IIS:\Sites\$($SiteInfo.Name)"
		if (-Not (Test-Path $address))
		{
			Write-Host "Creating new website..."
			New-WebSite -Name $SiteInfo.Name -Port $SiteInfo.Port -Force
		}

		Write-Host "Setting website properties..."
		Set-ItemProperty $address ApplicationPool $SiteInfo.AppPoolName
		Set-ItemProperty $address PhysicalPath $SiteInfo.PhysicalPath
	}
}

function Start-WebMaintenance()
{
	param ($ComputerInfo, $WebInfo)
	if (-Not $WebInfo.Deploy) { return }
	Log-Info 'Starting WebApp maintenance mode...'
	Ensure-RemotingSession $ComputerInfo
	$pathExists = Invoke-Command -Session $ComputerInfo.Session -ArgumentList $WebInfo -ScriptBlock {
		param ($WebInfo)
		return (Test-Path $WebInfo.AppPhysicalPath)
	}
	if ($pathExists)
	{
		Copy-Item 'app_offline.htm' -Destination $WebInfo.AppPhysicalPath -ToSession $ComputerInfo.Session
	}
}

function Stop-WebMaintenance()
{
	param ($ComputerInfo, $WebInfo)
	if (-Not $WebInfo.Deploy) { return }
	Log-Info 'Stopping WebApp maintenance mode...'
	Ensure-RemotingSession $ComputerInfo
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $WebInfo -ScriptBlock {
		param ($WebInfo)
		pushd $WebInfo.AppPhysicalPath
		if (Test-Path 'app_offline.htm')
		{
			Remove-Item 'app_offline.htm'
		}
		popd
	}
}