#
# NewAdUser.ps1
#

param ($Username, $FullName, $UserPrincipalName, $Password, $Path)

Import-Module activedirectory
New-ADUser `
-SamAccountName $Username `
-Name $FullName `
-UserPrincipalName $UserPrincipalName `
-AccountPassword (ConvertTo-SecureString -AsPlainText $Password -Force) `
-Enabled $true `
-PasswordNeverExpires $true `
-Path $Path