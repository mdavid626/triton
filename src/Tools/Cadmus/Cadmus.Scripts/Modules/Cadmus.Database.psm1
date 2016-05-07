#
# Cadmus.Database.psm1
#

function Parametrize-DbUpConfig
{
	param ($DbInfo)
	Replace-XmlValue 'DbUp/DbUp.exe.config' "/configuration/connectionStrings/add[@name='DefaultConnection']/@connectionString" $DbInfo.ConnectionString
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

