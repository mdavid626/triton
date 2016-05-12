#
# Cadmus.Report.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -DisableNameChecking

function Parametrize-DataSourceFile
{
	param ($File, $ReportInfo)
	ls $File | ForEach-Object {
		Replace-XmlValue $_ "/RptDataSource/ConnectionProperties/ConnectString" $ReportInfo.ConnectionString
	}
}

function Deploy-Report()
{
	param ($ComputerInfo, $ReportInfo)
	if (-Not $ReportInfo.Deploy) { return }
	Log-Info "Deploying reports..."
	Start-Verbose
	Ensure-RemotingSession $ComputerInfo

	Log-Info "Copy files to report server"

	Parametrize-DataSourceFile -File "$($ReportInfo.Name)\*.rds" -ReportInfo $ReportInfo
	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $ReportInfo
	Log-Info "Temp directory: $($ReportInfo.TempDir)"
	Copy-Item "$($ReportInfo.Name)\*" -Destination $ReportInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "PsScripts\Deploy-SSRSProject.ps1" -Destination $ReportInfo.TempDir -ToSession $ComputerInfo.Session
	Copy-Item "PsScripts\New-SSRSWebServiceProxy.ps1" -Destination $ReportInfo.TempDir -ToSession $ComputerInfo.Session

	Invoke-Command -Session $ComputerInfo.Session -ArgumentList $ReportInfo -ScriptBlock {
		param ($ReportInfo)
		
		pushd $ReportInfo.TempDir
		.\Deploy-SSRSProject.ps1 -Path ((ls *.rptproj).Name) -Verbose `
           -ServerUrl $ReportInfo.ServerURL -Folder $ReportInfo.Folder -DataSourceFolder $ReportInfo.DataSourceFolder `
		   -DataSetFolder $ReportInfo.DataSetFolder -OverwriteDataSources

		popd
		Remove-Item -Recurse -Force $ReportInfo.TempDir
	}
	Stop-Verbose
}