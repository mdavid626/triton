@echo off
setlocal

cd bin\debug
mklink /d Modules ..\..\..\Cadmus.Scripts\Modules
mklink /d SqlScripts ..\..\..\Cadmus.Scripts\SqlScripts
mklink Run.cmd ..\..\..\Cadmus.Scripts\Run.cmd
mklink Deploy.ps1 ..\..\..\Cadmus.Scripts\Deploy.ps1
mklink app_offline.htm ..\..\..\Cadmus.Scripts\app_offline.htm
mklink /d WebPackage ..\..\..\..\..\Nyx\Nyx.Web\Package
mklink /d DbUp ..\..\..\..\..\Nyx\Nyx.DbUp\bin\Debug
pause