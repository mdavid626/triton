#
# Cadmus.Foundation.psm1
#

function Write-Success()
{
	param ([string] $Message)

	if ([Console]::IsOutputRedirected) 
	{
		Write-Host "{Success}$Message"
	} 
	else
	{
		Write-Host $Message -ForegroundColor Green
	}
}

function Write-Header()
{
	param ([string] $Message)

	if ([Console]::IsOutputRedirected) 
	{
		Write-Host "{Header}$Message"
	} 
	else
	{
		Write-Host $Message -ForegroundColor Cyan
	}
}

function Write-Warn()
{
	param ([string] $Message)

	if ([Console]::IsOutputRedirected) 
	{
		Write-Host "{Warning}$Message"
	} 
	else
	{
		Write-Host $Message -ForegroundColor Yellow
	}
}

#Set-Item -Path WSMan:localhostClientTrustedHosts -Value ''