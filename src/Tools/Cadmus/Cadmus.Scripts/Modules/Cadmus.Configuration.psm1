#
# Cadmus.Configuration.psm1
#

function Load-Configuration()
{
	param ([string] $Path)
	Add-Type -Path 'Cadmus.Parametrizer.dll'
	$loader = New-Object -TypeName 'Cadmus.Parametrizer.ConfigManager' -ArgumentList $Path
	$loader.Load()
	return $loader
}

function Load-ComputerConfig()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	$computerName = $Config["${Name}Name"]
	$authMode = $Config["${Name}AuthMode"]
	$username = $Config["${Name}Username"]
	$password = $Config.GetSecureValue("${Name}Password")
	if ($password) 
	{
		$cred = New-Object System.Management.Automation.PSCredential ($username, $password)
	}
	
	return @{
		'ComputerName' = $computerName;
		'Authentication' = $authMode;
		'Username' = $username;
		'Credential' = $cred;
	}
}