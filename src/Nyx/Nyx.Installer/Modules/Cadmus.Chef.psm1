#
# Cadmus.Chef.psm1
#

Import-Module './Modules/Cadmus.Foundation.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Remoting.psm1' -DisableNameChecking
Import-Module './Modules/Cadmus.Configuration.psm1' -DisableNameChecking

function Parametrize-ChefAttribute
{
	param ($File, $Attribute, $Value, [switch] $Quotes)
	$lines = [System.IO.File]::ReadAllLines($File)
	$change = $false
	for ($i = 0; $i -lt $lines.Length; $i++)
	{
		if ($lines[$i].StartsWith($Attribute))
		{
			$newLine = $Attribute + " = "
			if ($Quotes) { $newLine = $newLine + "'" + $Value + "'" }
			else { $newLine = $newLine + $Value }
			if ($newLine -ne $lines[$i])
			{
				$lines[$i] = $newLine
				$change = $true
			}
		}
	}
	if ($change)
	{
		[System.IO.File]::WriteAllLines($File, $lines)
	}
}

function Parametrize-Chef
{
	param ($ComputerInfo, $ChefInfo)
	$file = 'Cookbooks/cadmus/attributes/default.rb'
	Parametrize-ChefAttribute -File $file -Attribute "default['dotnetframework']['version']" -Value $ChefInfo.DotNetVersion -Quotes
	Parametrize-ChefAttribute -File $file -Attribute "default['dotnetframework']['4.6.1']['url']" -Value $ChefInfo.DotNetUrl -Quotes

	$file = 'Cookbooks/cadmus/attributes/web.rb'
	Parametrize-ChefAttribute -File $file -Attribute "default['cadmus']['webdeploy']['url']" -Value $ChefInfo.WebDeployURL -Quotes
	Parametrize-ChefAttribute -File $file -Attribute "default['cadmus']['user']['create']" -Value $ChefInfo.WebUserCreate.ToString().ToLower()
	Parametrize-ChefAttribute -File $file -Attribute "default['cadmus']['user']['username']" -Value $ChefInfo.WebUserUsername -Quotes
	Parametrize-ChefAttribute -File $file -Attribute "default['cadmus']['user']['password']" -Value $ChefInfo.WebUserPassword -Quotes

	if ($ComputerInfo.ConfigName -eq 'SqlServer')
	{
		$file = 'Cookbooks/cadmus/attributes/sql.rb'
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['instance_name']" -Value $ChefInfo.SqlInstance -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['version']" -Value $ChefInfo.SqlVersion -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['product_key']" -Value $ChefInfo.SqlProductKey -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['port']" -Value $ChefInfo.SqlPort
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['feature_list']" -Value 'SQLENGINE,SNAC_SDK' -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['server']['installerurl']" -Value $ChefInfo.SqlInstallerUrl -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['server']['dotnetsources']" -Value $ChefInfo.DotNet35SourcesUrl -Quotes
	}
	else
	{
		$file = 'Cookbooks/cadmus/attributes/sql.rb'
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['instance_name']" -Value $ChefInfo.ReportInstance -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['version']" -Value $ChefInfo.ReportVersion -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['product_key']" -Value $ChefInfo.ReportProductKey -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['port']" -Value $ChefInfo.ReportPort
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['feature_list']" -Value 'SQLENGINE,RS,SNAC_SDK' -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['server']['installerurl']" -Value $ChefInfo.SqlInstallerUrl -Quotes
		Parametrize-ChefAttribute -File $file -Attribute "default['sql_server']['server']['dotnetsources']" -Value $ChefInfo.DotNet35SourcesUrl -Quotes
	}
}

function Deploy-Chef
{
	param ($ComputerInfo, $ChefInfo)
	if (-Not $ChefInfo.Deploy -or -Not $ComputerInfo.ConfigChef) { return }

	Log-Info "Applying Chef configuration to $($ComputerInfo.ConfigName) - $($ComputerInfo.Name)..."

	Ensure-RemotingSession $ComputerInfo
	Log-Info "Creating temp directory..."
	Ensure-RemoteTempDirectory -Session $ComputerInfo.Session $ChefInfo
	Log-Info "Temp directory: $($ChefInfo.TempDir)"

	Parametrize-Chef -ComputerInfo $ComputerInfo -ChefInfo $ChefInfo

	Log-Info "Copying cookbooks..."
	$ComputerInfo.ChefCookbooks | Foreach-Object {
		Write-Host "Copying $_..."
		Copy-Item "Cookbooks\$_" -Filter "*" -Destination "$($ChefInfo.TempDir)\Cookbooks\$_\" `
		          -ToSession $ComputerInfo.Session -Recurse
	}

	$ChefInfo.Recipes = $ComputerInfo.ChefRecipes -join ','
	if (-Not [string]::IsNullOrEmpty($ChefInfo.Recipes))
	{
		Invoke-Command -Session $ComputerInfo.Session -ArgumentList $ChefInfo -ScriptBlock {
			param ($ChefInfo)
			pushd  $ChefInfo.TempDir

			Write-Host "Checking Chef installation..."
			if (-Not (Test-Path "C:\opscode\chef\bin\chef-client.bat"))
			{
				Write-Host "Downloading Chef..."
				wget $ChefInfo.ChefClientInstallerUrl -OutFile "chef-client.msi"
				Write-Host "Installing Chef..."
				$path = (Get-ChildItem "chef-client.msi").FullName
				$args = @('/i', "$path", '/qn', '/passive')   
				$proc = Start-Process msiexec -NoNewWindow -Wait -ArgumentList $args -PassThru -ErrorAction Stop
				if ($proc.ExitCode -ne 0) {
					throw "Chef-Client instalation failed: $($proc.ExitCode)"   
				}
			}

			Write-Host "Starting chef-client..."
			& C:\opscode\chef\bin\chef-client.bat -z -o $ChefInfo.Recipes 2>1
			if ($LastExitCode -ne 0)
			{
				Start-Sleep 1
				throw "Chef failed: $LastExitCode"
			}

			popd
			Remove-Item -Recurse -Force $ChefInfo.TempDir
		}
	}
}