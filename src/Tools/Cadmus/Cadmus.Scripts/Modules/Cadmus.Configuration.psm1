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
		'ConfigName' = $Name;
		'Name' = $computerName;
		'Authentication' = $authMode;
		'Username' = $username;
		'Credential' = $cred;
		'ConfigChef' = ([System.Convert]::ToBoolean($Config["ChefConfig${Name}"]));
		"ChefCookbooks" = $Config.GetMultiValue("${Name}Cookbooks")
		"ChefRecipes" = $Config.GetMultiValue("${Name}Recipes")
	}
}

function Load-MultiComputerInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	$info = Load-ComputerInfo -Name $Name -Config $Config
	$computerNames = $Config.GetMultiValue("ClientComputersName")
	$computers = $computerNames | ForEach-Object { @{ 
		'ConfigName' = $Name;
		'Name' = $_;
		'Authentication' = $info.Authentication;
		'Username' = $info.Username;
		'Credential' = $info.Credential;
		'ConfigChef' = ([System.Convert]::ToBoolean($Config["ChefConfig${Name}"]));
		"ChefCookbooks" = $Config.GetMultiValue("${Name}Cookbooks")
		"ChefRecipes" = $Config.GetMultiValue("${Name}Recipes")
	} }
	return $computers
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
		'AppPoolName' = $Config["${Name}AppPoolName"];
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
		'WebPassword' = $Config.GetUnProtectedValue("${Name}WebPassword");
		'Backup' = ([System.Convert]::ToBoolean($Config["${Name}Backup"]));
		'Account' = ([System.Convert]::ToBoolean($Config["${Name}Account"]));
	}
}

function Load-SchedulerInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	return @{
		'Deploy' = ([System.Convert]::ToBoolean($Config["${Name}Deploy"]));
		'Name' = $Name;
		'Path' = $Config["${Name}Path"];
		'TaskName' = $Config["${Name}TaskName"];
		'TaskShutdownName' = $Config["${Name}TaskName"] + "_Shutdown";
		'ConnectionString' = $Config["${Name}ConnectionString"];
		'Username' = $Config["${Name}Username"];
		'Password' = $Config["${Name}Password"];
	}
}

function Load-MsiInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	return @{
		'Deploy' = ([System.Convert]::ToBoolean($Config["${Name}Deploy"]));
		'Name' = $Name;
	}
}

function Load-ReportInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	return @{
		'Deploy' = ([System.Convert]::ToBoolean($Config["${Name}Deploy"]));
		'Name' = $Name;
		'ServerURL' = $Config["${Name}ServerUrl"];
		'Folder' = $Config["${Name}Folder"];
		'DataSourceFolder' = $Config["${Name}DataSourceFolder"];
		'DataSetFolder' = $Config["${Name}DataSetFolder"];
		'ConnectionString' = $Config["${Name}ConnectionString"];
	}
}

function Load-SiteInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	return @{
		'Deploy' = ([System.Convert]::ToBoolean($Config["${Name}Deploy"]));
		'Name' = $Config["${Name}Name"];
		'Port' = $Config["${Name}Port"];
		'PhysicalPath' = $Config["${Name}PhysicalPath"];
		'RemoveDefault' = ([System.Convert]::ToBoolean($Config["${Name}RemoveDefault"]));
		'AppPoolName' = $Config["${Name}AppPoolName"];
		'AppPoolIdentity' = ([System.Convert]::ToInt32($Config["${Name}AppPoolIdentity"]));
		'AppPoolDeploy' = ([System.Convert]::ToBoolean($Config["${Name}AppPoolDeploy"]));
		'AppPoolUsername' = $Config["${Name}AppPoolUsername"];
		'AppPoolPassword' = $Config["${Name}AppPoolPassword"];
	}
}

function Load-ChefInfo()
{
	param ([string] $Name, [Cadmus.Parametrizer.ConfigManager] $Config)
	return @{
		'Deploy' = ([System.Convert]::ToBoolean($Config["${Name}Deploy"]));
		'ChefClientInstallerUrl' = $Config["${Name}ChefClientInstallerUrl"];
		'WebDeployUrl' = $Config["${Name}WebDeployUrl"];
		'DotNetVersion' = $Config["${Name}DotNetVersion"];
		'DotNetUrl' = $Config["${Name}DotNetUrl"];
		'WebUserCreate' = ([System.Convert]::ToBoolean($Config["${Name}WebUserCreate"]));;
		'WebUserUsername' = $Config["${Name}WebUserUsername"];
		'WebUserPassword' = $Config.GetUnProtectedValue("${Name}WebUserPassword");
		'SqlInstance' = $Config["${Name}SqlInstance"];
		'SqlVersion' = $Config["${Name}SqlVersion"];
		'SqlProductKey' = $Config.GetUnProtectedValue("${Name}SqlProductKey");
		'SqlPort' = $Config["${Name}SqlPort"];
		'ReportInstance' = $Config["${Name}ReportInstance"];
		'ReportVersion' = $Config["${Name}ReportVersion"];
		'ReportProductKey' = $Config.GetUnProtectedValue("${Name}ReportProductKey");
		'ReportPort' = $Config["${Name}ReportPort"];
	}
}

function Replace-XmlValue()
{
	param ([string] $Path, [string] $Match, [string] $Value)
	Add-Type -Path 'Cadmus.Parametrizer.dll'
	$replacer = New-Object -TypeName 'Cadmus.Parametrizer.XmlReplacer'
	$replacer.Replace($Path, $Match, $Value)
}