#
# Deploy.ps1
#

# Arguments
param (
	[string] $Action = 'Deploy',
	[string] $ConfigurationFile = 'config.xml'
)

# Imports
Import-Module './Modules/Cadmus.Foundation.psm1' -Force -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -Force -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -Force -DisableNameChecking
Import-Module './Modules/Cadmus.Web.psm1' -Force -DisableNameChecking

# Starting up
Show-BigHeader 'Starting Cymric Installer'
Log-Info "Action: $Action"
Log-Info "ConfigurationFile: $ConfigurationFile"
$ErrorActionPreference = "Stop"

# Configuration
Show-BigHeader "Loading configuration"
$config = Load-Configuration -Path $ConfigurationFile
$appServer = Load-ComputerInfo -Config $config -Name 'AppServer'
$sqlServer = Load-ComputerInfo -Config $config -Name 'SqlServer'
$computers = ($appServer, $sqlServer)
$web = Load-WebInfo -Config $config -Name 'Web'
Log-Info "$($config.Config.Parameters.Count) parameters loaded"

#$servers = ($config['AppServerName'], $config['SqlServerName'])
#$clients = $config.GetMultiValue('ClientComputerNames')
#$computers = $servers + $clients

# Actions
Show-BigHeader "Performing action $Action"

if ($Action -eq 'CheckServersConnection') 
{
	Test-RemotingConnection $computers
}

if ($Action -eq 'CheckServersAuth') 
{
	Test-RemotingAuth $computers
}

if ($Action -eq 'DeployWeb')
{
	Deploy-WebApp -ComputerInfo $appServer -WebInfo $web
}
