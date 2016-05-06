#
# Deploy.ps1
#

param (
	[string] $Action
)

Write-Output 'Starting Deploy.ps1'
Write-Output "Action $Action"

Import-Module './Modules/Cadmus.Foundation.psm1' -Force -DisableNameChecking
Write-Header 'header'

Write-Success 'success'
Write-Header 'header'
Write-Warn "warning"
Write-Error "hellooo errro"

$length = $args.Length;
Write-Host "arguments: $length"

foreach ($arg in $args)
{
	Write-Host "Argument: $arg"
}