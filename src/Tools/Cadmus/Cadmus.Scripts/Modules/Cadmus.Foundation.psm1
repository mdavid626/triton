#
# Cadmus.Foundation.psm1
#

Add-Type -Path 'Cadmus.Foundation.dll'
$logger = New-Object -TypeName 'Cadmus.Foundation.ConsoleLogger'

function Log-Info() 
{
	param ([string] $Message)
	$logger.LogInfo($Message)
}

function Log-Success() 
{
	param ([string] $Message)
	$logger.LogSuccess($Message)
}

function Log-Warning() 
{
	param ([string] $Message)
	$logger.LogWarning($Message)
}

function Log-Error() 
{
	param ([string] $Message)
	$logger.LogError($Message)
}

function Log-Verbose()
{
	param ([string] $Message)
	$logger.LogVerbose($Message)
}

function Log-Header() 
{
	param ([string] $Message)
	$logger.LogHeader($Message)
}

function Start-Verbose()
{
	$logger.StartVerbose()
}

function Stop-Verbose()
{
	$logger.StopVerbose()
}

function Show-BigHeader()
{
	param ([string] $Header)
	Log-Header '=================================================='
	Log-Header $Header
	Log-Header '=================================================='
}

function Test-RemotingConnection()
{
	param ([string[]] $ComputerNames)
	foreach ($name in $ComputerNames)
	{
		try
		{
			if ([String]::IsNullOrEmpty($name)) 
			{
				continue
			}
			Log-Info "Connecting ${name}..."
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
	param ([string[]] $Names, [Cadmus.Parametrizer.ConfigManager] $Config)
	foreach ($name in $Names)
	{
		try
		{
			if ([String]::IsNullOrEmpty($name)) 
			{
				continue
			}
			Start-Verbose
			$cc = Load-ComputerConfig -Name $name -Config $Config

			if ($cc.Credential) 
			{
				$result = Test-WSMan -ComputerName $cc.ComputerName -Credential $cc.Credential -Authentication $cc.Authentication
			} 
			else 
			{
				$result = Test-WSMan -ComputerName $cc.ComputerName -Authentication $cc.Authentication
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

#Set-Item -Path WSMan:localhostClientTrustedHosts -Value ''