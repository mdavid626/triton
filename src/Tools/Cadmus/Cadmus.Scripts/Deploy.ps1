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

# Starting up
Show-BigHeader 'Starting Cymric Installer'
Log-Info "Action: $Action"
Log-Info "ConfigurationFile: $ConfigurationFile"
$ErrorActionPreference = "Stop"

# Configuration
Show-BigHeader "Loading configuration"
$config = Load-Configuration -Path $ConfigurationFile
$servers = ($config['AppServerName'], $config['SqlServerName'])
$clients = $config.GetMultiValue('ClientComputerNames')
$computers = $servers + $clients
Log-Info "$($config.Config.Parameters.Count) parameters loaded"

# Actions
Show-BigHeader "Performing action $Action"

# CheckServers
if ($Action -eq 'CheckServersConnection') 
{
	Test-RemotingConnection $computers
}

if ($Action -eq 'CheckServersAuth') 
{
	Test-RemotingAuth -Names ('AppServer', 'SqlServer') -Config $config
}
