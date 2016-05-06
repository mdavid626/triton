#
# Run.ps1
#

$allArgs = $PsBoundParameters.Values + $args
.\Deploy.ps1 $allArgs
#Get-Process | Tee-Object -FilePath 'deploy.log'
