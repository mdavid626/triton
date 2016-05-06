#
# Deploy.ps1
#

param (
	[string] $Action
)

Import-Module './Modules/Cadmus.Foundation.psm1' -Force -DisableNameChecking

Log-Info 'Starting Run.ps1'
Log-Info "Action $Action"


.\debug\dbup.exe

Log-Header 'header'
Log-Success 'success'
Log-Warning "warning"
Log-Error "hellooo errro"
Log-Verbose 'verbose'

#Get-Process

Start-Verbose
Log-Info 'hello verbose mode'
Stop-Verbose
Log-Info 'end'