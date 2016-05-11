#
# JoinAd.ps1
#

param ($DnsIp, $DomainName)
$secpasswd = ConvertTo-SecureString "vagrant" -AsPlainText -Force
$mycreds = New-Object System.Management.Automation.PSCredential ("vagrant", $secpasswd)
Set-DnsClientServerAddress -ServerAddresses $DnsIp -InterfaceAlias 'Ethernet 2'
Add-Computer -DomainName $DomainName -Force -Credential $mycreds

