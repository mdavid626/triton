#
# Run.ps1
#

.\Deploy.ps1 $args | Tee-Object -FilePath 'deploy.log'
