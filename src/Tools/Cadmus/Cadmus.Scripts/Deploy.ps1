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
Import-Module './Modules/Cadmus.Database.psm1' -Force -DisableNameChecking
Import-Module './Modules/Cadmus.Scheduler.psm1' -Force -DisableNameChecking

# Starting up
Show-BigHeader 'Starting Cymric Installer'
$stopper = [System.Diagnostics.StopWatch]::StartNew()
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
$db = Load-DbInfo -Config $config -Name 'Db'
$scheduler = Load-SchedulerInfo -Config $config -Name 'Scheduler'
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

if ($Action -eq 'CreateDatabase')
{
	Create-Database -ComputerInfo $sqlServer -DbInfo $db
}

if ($Action -eq 'DropDatabase')
{
	Drop-Database -ComputerInfo $sqlServer -DbInfo $db
}

if ($Action -eq 'MigrateDatabase')
{
	Migrate-Database -ComputerInfo $sqlServer -DbInfo $db
}

if ($Action -eq 'DeployScheduler')
{
	Deploy-Scheduler -ComputerInfo $appServer -SchedulerInfo $scheduler
}

if ($Action -eq 'Deploy')
{
	Start-WebMaintenance -ComputerInfo $appServer -WebInfo $web
	Start-SchedulerMaintenance -ComputerInfo $appServer -SchedulerInfo $scheduler

	Deploy-WebApp -ComputerInfo $appServer -WebInfo $web
	Deploy-Scheduler -ComputerInfo $appServer -SchedulerInfo $scheduler

	Backup-Database $sqlServer $db
	Migrate-Database -ComputerInfo $sqlServer -DbInfo $db
	Setup-DbUserAccount $sqlServer $db

	Stop-SchedulerMaintenance -ComputerInfo $appServer -SchedulerInfo $scheduler
	Stop-WebMaintenance -ComputerInfo $appServer -WebInfo $web
}

# Ending...
$stopper.Stop()
Show-BigHeader "Finished in $($stopper.Elapsed)"
