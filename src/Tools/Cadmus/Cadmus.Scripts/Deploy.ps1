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
Import-Module './Modules/Cadmus.Msi.psm1' -Force -DisableNameChecking
Import-Module './Modules/Cadmus.Report.psm1' -Force -DisableNameChecking
Import-Module './Modules/Cadmus.Chef.psm1' -Force -DisableNameChecking

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
$reportServer = Load-ComputerInfo -Config $config -Name 'ReportServer'
$clients = Load-MultiComputerInfo -Config $config -Name 'ClientComputers'
$computers = ($appServer, $sqlServer, $reportServer)
$web = Load-WebInfo -Config $config -Name 'Web'
$site = Load-SiteInfo -Config $config -Name 'Site'
$db = Load-DbInfo -Config $config -Name 'Db'
$scheduler = Load-SchedulerInfo -Config $config -Name 'Scheduler'
$clientTools = Load-MsiInfo -Config $config -Name 'ClientTools'
$report = Load-ReportInfo -Config $config -Name 'Report'
$chef = Load-ChefInfo -Config $config -Name 'Chef'
Log-Info "$($config.Config.Parameters.Count) parameters loaded"

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

if ($Action -eq 'StartWebMaintenance')
{
	Start-WebMaintenance -ComputerInfo $appServer -WebInfo $web
}

if ($Action -eq 'StopWebMaintenance')
{
	Stop-WebMaintenance -ComputerInfo $appServer -WebInfo $web
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

if ($Action -eq 'StartSchedulerMaintenance')
{
	Start-SchedulerMaintenance -ComputerInfo $appServer -SchedulerInfo $scheduler
}

if ($Action -eq 'StopSchedulerMaintenance')
{
	Stop-SchedulerMaintenance -ComputerInfo $appServer -SchedulerInfo $scheduler
}

if ($Action -eq 'DeployClientTools')
{
	Deploy-Msi -ComputerInfo $clients -MsiInfo $clientTools
}

if ($Action -eq 'DeployReport')
{
	Deploy-Report -ComputerInfo $reportServer -ReportInfo $report
}

if ($Action -eq 'DeployWebSite')
{
	Deploy-WebSite -ComputerInfo $appServer -SiteInfo $site
}

if ($Action -eq 'DeployChef')
{
	$computers | Foreach-Object { Deploy-Chef -ComputerInfo $_ -ChefInfo $chef }
}

if ($Action -eq 'Deploy')
{
	Start-WebMaintenance -ComputerInfo $appServer -WebInfo $web
	Start-SchedulerMaintenance -ComputerInfo $appServer -SchedulerInfo $scheduler

	$computers | Foreach-Object { Deploy-Chef -ComputerInfo $_ -ChefInfo $chef }
	Deploy-WebSite -ComputerInfo $appServer -SiteInfo $site
	Deploy-WebApp -ComputerInfo $appServer -WebInfo $web
	Deploy-Scheduler -ComputerInfo $appServer -SchedulerInfo $scheduler

	Backup-Database $sqlServer $db
	Migrate-Database -ComputerInfo $sqlServer -DbInfo $db
	Setup-DbUserAccount $sqlServer $db

	Deploy-Report -ComputerInfo $reportServer -ReportInfo $report
	Deploy-Msi -ComputerInfo $clients -MsiInfo $clientTools

	Stop-SchedulerMaintenance -ComputerInfo $appServer -SchedulerInfo $scheduler
	Stop-WebMaintenance -ComputerInfo $appServer -WebInfo $web
}

# Ending...
$stopper.Stop()
Show-BigHeader "Finished in $($stopper.Elapsed)"
