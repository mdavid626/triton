#
# Cadmus.Remoting.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking

function Test-RemotingConnection()
{
	param ($ComputerInfos)
	foreach ($info in $ComputerInfos)
	{
		try
		{
			$name = $info.Name
			if ([String]::IsNullOrEmpty($name)) 
			{
				continue
			}
			Log-Info "Connecting ${$name}..."
			Start-Verbose
			Test-WSMan -ComputerName $name
			Log-Success "${name}: OK"
		}
		catch
		{
			Log-Error "${name}: Failed"
		}
		finally
		{
			Stop-Verbose
		}
	}
}

function Test-RemotingAuth()
{
	param ($ComputerInfos)
	Log-Info "Connecting..."
	foreach ($info in $ComputerInfos)
	{
		try
		{
			$name = $info.Name
			if ([String]::IsNullOrEmpty($name)) 
			{
				continue
			}
			Start-Verbose

			if ($info.Credential) 
			{
				$result = Test-WSMan -ComputerName $name -Credential $info.Credential -Authentication $info.Authentication
			} 
			else 
			{
				$result = Test-WSMan -ComputerName $name -Authentication $info.Authentication
			}

			if ($result.ProductVersion.StartsWith('OS: 0.0.0'))
			{
				throw "Failed"
			}
			
			Log-Success "${name}: OK"
		}
		catch
		{
			Log-Error "${name}: Failed"
		}
		finally
		{
			Stop-Verbose
		}
	}
}

function New-RemotingSession 
{
	param ($ComputerInfo)
	if ($ComputerInfo.Credential) 
	{
		$session = New-PSSession -ComputerName $ComputerInfo.Name -Credential $ComputerInfo.Credential -Authentication $ComputerInfo.Authentication
	} 
	else 
	{
		$session = New-PSSession -ComputerName $ComputerInfo.Name -Authentication $ComputerInfo.Authentication
	}
	return $session
}

function Ensure-RemotingSession
{
	param ($ComputerInfo)
	if (-Not $ComputerInfo.Session)
	{
		$ComputerInfo.Session = New-RemotingSession $ComputerInfo
	}
}

function New-RemoteTempDirectory
{
	param ($Session)
	$tempDir = Invoke-Command -Session $Session -ScriptBlock {
		$tempDir = "Cymric_" + [System.Guid]::NewGuid().ToString()
		$result = New-Item -ItemType Directory -Name $tempDir -Path $env:TEMP 
		return $result.FullName
	}
	return $tempDir
}

function Ensure-RemoteTempDirectory
{
	param ($Session, $Info)
	$Info.TempDir = New-RemoteTempDirectory -Session $Session
}

function Prepare-UnProtect
{
	param ($Session, $Info)
	Ensure-RemoteTempDirectory -Session $Session -Info $Info
	Copy-Item 'Cadmus.Foundation.dll' -Destination $Info.TempDir -ToSession $Session
	$Info.UnProtectDll = [System.IO.Path]::Combine($Info.TempDir, 'Cadmus.Foundation.dll')
}