#
# JoinAd.ps1
#

param ($AdIp, $DomainName, $AdminUserName, $AdminPassword)

Set-DnsClientServerAddress -ServerAddresses $AdIp -InterfaceAlias 'Ethernet 2'
Enable-NetFirewallRule -DisplayName 'Windows Remote Management (HTTP-In)'

$secpasswd = ConvertTo-SecureString $AdminPassword -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ($AdminUserName, $secpasswd)
Add-Computer -DomainName $DomainName -Force -Credential $mycreds

Restart-Computer
