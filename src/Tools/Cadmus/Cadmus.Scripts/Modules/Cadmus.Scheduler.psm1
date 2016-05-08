#
# Cadmus.Scheduler.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -DisableNameChecking

function Deploy-Scheduler()
{
	param ($ComputerInfo, $SchedulerInfo)
	if (-Not $SchedulerInfo.Deploy) { return }
	Log-Info "Deploying Scheduler..."
}

function Start-SchedulerMaintenance()
{
	Log-Info 'Starting Scheduler maintenance mode...'
}

function Stop-SchedulerMaintenance()
{
	Log-Info 'Stopping Scheduler maintenance mode...'
}