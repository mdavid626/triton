#
# Cadmus.Database.psm1
#

function Parametrize-SqlScript 
{
	param ($Script, $Parameters)
	$result = $Script
	if ($Parameters.Keys)
	{
		$Parameters.Keys | % { 
			$result = $result.Replace('$(' + $_ + ')', $Parameters.Item($_));
		}
	}
	return $result
}

function Invoke-SqlScript
{
	param ($ScriptPath, $ConnectionString, $Parameters, $Session)
	$script = [IO.File]::ReadAllText($ScriptPath)
    $command = Parametrize-SqlScript -Script $script -Parameters $Parameters
	$subcmds = $command -split "GO"
	Invoke-Command -Session $Session -ArgumentList ($ConnectionString, $subcmds) -ScriptBlock {
		param ($ConnectionString, $Commands)
		foreach ($command in $Commands)
		{
			$sqlConnection = New-Object System.Data.SqlClient.SqlConnection
			$sqlConnection.ConnectionString = $ConnectionString
			$sqlConnection.FireInfoMessageEventOnUserErrors = $true
			$handler = [System.Data.SqlClient.SqlInfoMessageEventHandler] {
				Write-Host "$($_)"
			}
			$sqlConnection.add_InfoMessage($handler)
			$sqlConnection.Open()
			$sqlCommand = New-Object System.Data.SqlClient.SqlCommand
			$sqlCommand.CommandTimeout = 60
			$sqlCommand.Connection = $sqlConnection
			$sqlCommand.CommandText= $command
			$result = $sqlCommand.ExecuteNonQuery()
			$sqlCommand.Connection.Close()
		}
	}
}

function Parametrize-DbUpConfig
{
	param ($DbInfo)
	Replace-XmlValue 'DbUp/DbUp.exe.config' "/configuration/connectionStrings/add[@name='DefaultConnection']/@connectionString" $DbInfo.ConnectionString
}

function Setup-DbUserAccount
{
	param ($ComputerInfo, $DbInfo)
	if (-Not [string]::IsNullOrEmpty($DbInfo.WebUsername))
	{
		Log-Info "Checking user account $($DbInfo.WebUsername).."
		Start-Verbose
		Ensure-RemotingSession $ComputerInfo
		$windows = if ($DbInfo.WebUsername.Contains('\')) { "1" } else { "0" }
		Invoke-SqlScript -Script 'SqlScripts/user.sql' -Session $ComputerInfo.Session `
						 -ConnectionString $DbInfo.ConnectionString -Parameters @{
			'Windows' =  $windows;
			'Username' = $DbInfo.WebUsername;
			'Password' = $DbInfo.WebPassword;
		}
		Stop-Verbose
	}
}

function Backup-Database()
{
	param ($ComputerInfo, $DbInfo)
	if ($DbInfo.Backup)
	{
		Log-Info "Backing up database..."
		Start-Verbose
		Ensure-RemotingSession $ComputerInfo
		Invoke-SqlScript -Script 'SqlScripts/backup.sql' -Session $ComputerInfo.Session `
						 -ConnectionString $DbInfo.ConnectionString
		Stop-Verbose
	}
}

function Create-Database
{
	param ($ComputerInfo, $DbInfo)
	Log-Info "Creating database..."
	Ensure-RemotingSession $ComputerInfo
	Parametrize-DbUpConfig $DbInfo

	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $DbInfo
	Log-Verbose "Temp directory: $($DbInfo.TempDir)"

	Copy-Item "DbUp\*.exe" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\*.dll" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\*.config" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\*.dacpac" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session

	Start-Verbose
	
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $DbInfo -ScriptBlock {
		param ($DbInfo)

		pushd $DbInfo.TempDir
		.\DbUp.Exe /create /silent
		if ($LastExitCode -ne 0) { throw "DbUp failed: $LastExitCode" }
		popd

		Remove-Item -Recurse -Force $DbInfo.TempDir
	}

	Setup-DbUserAccount $ComputerInfo $DbInfo

	Stop-Verbose
	Log-Success "Database successfully created."
}

function Migrate-Database
{
	param ($ComputerInfo, $DbInfo)
	Log-Info "Migrating database..."
	Ensure-RemotingSession $ComputerInfo
	Parametrize-DbUpConfig $DbInfo

	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $DbInfo
	Log-Verbose "Temp directory: $($DbInfo.TempDir)"

	Copy-Item "DbUp\*.exe" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\Cadmus*.dll" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\DbUp*.dll" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\Fluent*.dll" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\*.config" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session

	Start-Verbose
	
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $DbInfo -ScriptBlock {
		param ($DbInfo)

		pushd $DbInfo.TempDir
		.\DbUp.exe /upgrade /silent /transaction $DbInfo.TransactionLevel
		if ($LastExitCode -ne 0) { throw "DbUp failed: $LastExitCode" }
		popd

		Remove-Item -Recurse -Force $DbInfo.TempDir
	}
	Stop-Verbose
	Log-Success "Database successfully migrated."
}

function Drop-Database
{
	param ($ComputerInfo, $DbInfo)
	Log-Info "Dropping database..."
	Ensure-RemotingSession $ComputerInfo
	Parametrize-DbUpConfig $DbInfo

	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $DbInfo
	Log-Verbose "Temp directory: $($DbInfo.TempDir)"

	Copy-Item "DbUp\*.exe" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\Cadmus*.dll" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\DbUp*.dll" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\Fluent*.dll" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "DbUp\*.config" -Destination $DbInfo.TempDir -ToSession $ComputerInfo.Session

	Start-Verbose
	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $DbInfo -ScriptBlock {
		param ($DbInfo)

		pushd $DbInfo.TempDir
		.\DbUp.exe /drop /silent
		if ($LastExitCode -ne 0) { throw "DbUp failed: $LastExitCode" }
		popd

		Remove-Item -Recurse -Force $DbInfo.TempDir
	}
	Stop-Verbose
	Log-Success "Database successfully dropped."
}

