#
# Cadmus.Configuration.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking

function Load-Configuration()
{
	param ([string] $Path)
	Add-Type -Path 'Cadmus.Parametrizer.dll'
	$loader = New-Object -TypeName 'Cadmus.Parametrizer.ConfigManager' -ArgumentList $Path
	$loader.Load()
	return $loader
}

function Load-ComputerInfo()
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
		'Name' = $computerName;
		'Authentication' = $authMode;
		'Username' = $username;
		'Credential' = $cred;
	}
}

function Load-WebInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	$packagePath = $Config["${Name}PackagePath"];

	return @{
		'Deploy' = ([System.Convert]::ToBoolean($Config["${Name}Deploy"]));
		'PackageFolder' = [System.IO.Path]::GetDirectoryName($packagePath);
		'PackageDeployCmd' = "$($Name).deploy.cmd"
		'PackageParamsXml' = "$($Name).SetParameters.xml";
		'AppName' = $Config["${Name}AppName"];
		'AppPhysicalPath' = $Config["${Name}AppPhysicalPath"];
		'AppAuthMode' = $Config["${Name}AppAuthMode"];
		'SiteName' = $Config["${Name}SiteName"];
		'AppPath' = $Config["${Name}SiteName"] + "/" +$Config["${Name}AppName"];
		'MachineValidationKey' = $Config["WebMachineValidationKey"];
		'MachineDecryptionKey' = $Config["WebMachineDecryptionKey"];
		'ConnectionString' = $Config["WebConnectionString"];
	}
}

function Load-DbInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	return @{
		'Deploy' = ([System.Convert]::ToBoolean($Config["${Name}Deploy"]));
		'ConnectionString' = $Config["${Name}ConnectionString"];
		'TransactionLevel' = $Config["${Name}MigrationTransactionLevel"];
		'WebUsername' = $Config["${Name}WebUsername"];
		'WebPassword' = $Config["${Name}WebPassword"];
		'Backup' = ([System.Convert]::ToBoolean($Config["${Name}Backup"]));
	}
}

function Replace-XmlValue()
{
	param ([string] $Path, [string] $Match, [string] $Value)
	Add-Type -Path 'Cadmus.Parametrizer.dll'
	$replacer = New-Object -TypeName 'Cadmus.Parametrizer.XmlReplacer'
	$replacer.Replace($Path, $Match, $Value)
}