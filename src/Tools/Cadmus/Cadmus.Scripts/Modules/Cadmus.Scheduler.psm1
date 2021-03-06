#
# Cadmus.Scheduler.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -DisableNameChecking

function Parametrize-SchedulerConfig()
{
	param ($Folder, $SchedulerInfo)
	Replace-XmlValue "$Folder/Scheduler.exe.config" "/configuration/connectionStrings/add[@name='DefaultConnection']/@connectionString" $SchedulerInfo.ConnectionString
	Replace-XmlValue "$Folder/task_run.xml" "/ns0:Task/ns0:Actions/ns0:Exec/ns0:WorkingDirectory" $SchedulerInfo.Path
	Replace-XmlValue "$Folder/task_shutdown.xml" "/ns0:Task/ns0:Actions/ns0:Exec/ns0:WorkingDirectory" $SchedulerInfo.Path
}

function Deploy-Scheduler()
{
	param ($ComputerInfo, $SchedulerInfo)
	if (-Not $SchedulerInfo.Deploy) { return }
	Log-Info "Deploying Scheduler..."
	Start-Verbose
	Ensure-RemotingSession $ComputerInfo

	# Copy files
	Log-Info "Copying scheduler files"
	Parametrize-SchedulerConfig -Folder $SchedulerInfo.Name $SchedulerInfo
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $SchedulerInfo -ScriptBlock {
		param ($SchedulerInfo)
		if (-Not (Test-Path $SchedulerInfo.Path))
		{
			New-Item $SchedulerInfo.Path -ItemType Directory
		}

		Get-ChildItem -Path $SchedulerInfo.Path -Include * | Remove-Item -Recurse -Force

		$acl = Get-Acl -Path $SchedulerInfo.Path
		$perm = $SchedulerInfo.Username, 'ReadAndExecute, Synchronize', 'ContainerInherit, ObjectInherit', 'None', 'Allow' 
		$rule = New-Object -TypeName System.Security.AccessControl.FileSystemAccessRule -ArgumentList $perm
		$acl.SetAccessRule($rule) 
		$acl | Set-Acl -Path $SchedulerInfo.Path
	}

	Copy-Item "$($SchedulerInfo.Name)\Scheduler.exe" -Destination $SchedulerInfo.Path -ToSession $ComputerInfo.Session
	Copy-Item "$($SchedulerInfo.Name)\Scheduler.exe.config" -Destination $SchedulerInfo.Path -ToSession $ComputerInfo.Session
	Copy-Item "$($SchedulerInfo.Name)\task*.xml" -Destination $SchedulerInfo.Path -ToSession $ComputerInfo.Session
	Copy-Item "$($SchedulerInfo.Name)\*.dll" -Destination $SchedulerInfo.Path -ToSession $ComputerInfo.Session

	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $SchedulerInfo -ScriptBlock {
		param ($SchedulerInfo)
		$taskPath = [System.IO.Path]::Combine($SchedulerInfo.Path, 'task_run.xml')
		$taskShutdownPath = [System.IO.Path]::Combine($SchedulerInfo.Path, 'task_shutdown.xml')

		Add-Type -Path ([System.IO.Path]::Combine($SchedulerInfo.Path, 'Cadmus.Foundation.dll'))
		$protector = New-Object -TypeName 'Cadmus.Foundation.PasswordProtector'
		$password = $protector.UnProtect($SchedulerInfo.Password)
		
		# Task for running
		Write-Host "Searching for task $($SchedulerInfo.TaskName)"
		schtasks.exe /query /tn $SchedulerInfo.TaskName 2>1 > $null
		if ($LastExitCode -eq 0)
		{
			Write-Host "Task already exists, deleting..."
			schtasks.exe /delete /tn $SchedulerInfo.TaskName /f
		}
		Write-Host 'Adding new task'
		schtasks.exe /create /ru $SchedulerInfo.Username /rp "$password" /tn $SchedulerInfo.TaskName /xml $taskPath
		if ($LastExitCode -ne 0) { throw "Scheduler task creation failed." }

		# Task for shutdown
		Write-Host "Searching for task $($SchedulerInfo.TaskShutdownName)"
		schtasks.exe /query /tn $SchedulerInfo.TaskShutdownName 2>1 > $null
		if ($LastExitCode -eq 0)
		{
			Write-Host "Task already exists, deleting..."
			schtasks.exe /delete /tn $SchedulerInfo.TaskShutdownName /f
		}
		Write-Host 'Adding new task'
		schtasks.exe /create /ru $SchedulerInfo.Username /rp "$password" /tn $SchedulerInfo.TaskShutdownName /xml $taskShutdownPath
		if ($LastExitCode -ne 0) { throw "Scheduler task creation failed." }
	}
	Stop-Verbose
}

function Start-SchedulerMaintenance()
{
	param ($ComputerInfo, $SchedulerInfo)
	if (-Not $SchedulerInfo.Deploy) { return }
	Log-Info 'Starting Scheduler maintenance mode...'
	Start-Verbose
	Ensure-RemotingSession $ComputerInfo
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $SchedulerInfo -ScriptBlock {
		param ($SchedulerInfo)

		schtasks.exe /query /tn $SchedulerInfo.TaskShutdownName 2>1 > $null
		if ($LastExitCode -eq 0)
		{
			schtasks /run /tn $SchedulerInfo.TaskShutdownName
			for ($i = 0; $i -lt 10; $i++)
			{
				$job = Get-ScheduledTask -TaskName $SchedulerInfo.TaskName
				if ($job.State -ne 'Running')
				{
					schtasks /end /tn $SchedulerInfo.TaskShutdownName
					break
				}
				Write-Host "Waiting for $($SchedulerInfo.TaskName) to shut down..."
				Start-Sleep 1
			}
		}
		
		schtasks.exe /query /tn $SchedulerInfo.TaskName 2>1 > $null
		if ($LastExitCode -eq 0)
		{
			Disable-ScheduledTask -TaskName $SchedulerInfo.TaskName
		}
	}
	Start-Sleep 1
	Stop-Verbose
}

function Stop-SchedulerMaintenance()
{
	param ($ComputerInfo, $SchedulerInfo)
	if (-Not $SchedulerInfo.Deploy) { return }
	Log-Info 'Stopping Scheduler maintenance mode...'
	Start-Verbose
	Ensure-RemotingSession $ComputerInfo
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $SchedulerInfo -ScriptBlock {
		param ($SchedulerInfo)
		Enable-ScheduledTask -TaskName $SchedulerInfo.TaskName
		schtasks /run /tn $SchedulerInfo.TaskName
		if ($LastExitCode -ne 0)
		{
			throw "Scheduler running failed"
		}
	}
	Stop-Verbose
}