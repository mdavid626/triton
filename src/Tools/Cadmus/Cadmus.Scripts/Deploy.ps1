#
# Deploy.ps1
#

param (
	[string] $Action
)

Import-Module './Modules/Cadmus.Foundation.psm1' -Force -DisableNameChecking

Log-Header 'Cymric Installer'
Log-Info "Desired action: $Action"
Log-Info 'Deploying modules:'

Log-Header 'Web'

Log-Header 'header'
Log-Success 'success'
Log-Warning "warning"
Log-Error "hellooo errro"
Log-Verbose 'verbose'

Start-Verbose
Log-Info 'hello verbose mode'
Stop-Verbose
Log-Info 'end'