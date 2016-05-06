#
# Cadmus.Foundation.psm1
#

function Write-ColoredInfo()
{
	param ([string] $Message, [ConsoleColor] $Color, [string] $RedirectColor)
	if ([Console]::IsOutputRedirected) 
	{
		Write-Output "{$RedirectColor}$Message"
	} 
	else
	{
		Write-Host $Message -ForegroundColor $Color
	}
}

function Write-Success()
{
	param ([string] $Message)
	Write-ColoredInfo -Message $Message -Color Green -RedirectColor "Success"
}

function Write-Header()
{
	param ([string] $Message)
	Write-ColoredInfo -Message Cyan -Color Green -RedirectColor "Header"
}

function Write-Warn()
{
	param ([string] $Message)
	Write-ColoredInfo -Message $Message -Color Yellow -RedirectColor "Warning"
}

#Set-Item -Path WSMan:localhostClientTrustedHosts -Value ''