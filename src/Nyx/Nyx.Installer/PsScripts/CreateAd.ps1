#
# CreateAd.ps1
#

param ($DomainName, $AdminPassword)
Install-windowsfeature -name AD-Domain-Services –IncludeManagementTools

Import-Module ADDSDeployment

$password = ConvertTo-SecureString -AsPlainText $AdminPassword -Force

Install-ADDSForest `
-CreateDnsDelegation:$false `
-DatabasePath "C:\Windows\NTDS" `
-DomainMode "Win2012R2" `
-DomainName $DomainName `
-DomainNetbiosName $DomainName.Split('.')[0].ToUpper() `
-SafeModeAdministratorPassword $password `
-ForestMode "Win2012R2" `
-InstallDns: $true `
-LogPath "C:\Windows\NTDS" `
-NoRebootOnCompletion:$false `
-SysvolPath "C:\Windows\SYSVOL" `
-Force: $true
