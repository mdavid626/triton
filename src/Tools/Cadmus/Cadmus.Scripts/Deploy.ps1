#
# Deploy.ps1
#

Write-Host 'Starting Deploy.ps1'

Import-Module './Modules/Cadmus.Foundation.psm1' -Force -DisableNameChecking
Write-Header 'header'

Write-Success 'success'
Write-Header 'header'
Write-Warn "warning"

$length = $args.Length;
Write-Host "arguments: $length"

foreach ($arg in $args)
{
	Write-Host "Argument: $arg"
}