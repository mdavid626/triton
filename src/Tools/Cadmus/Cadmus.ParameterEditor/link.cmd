@echo off
setlocal

cd bin\debug
mklink /d Modules ..\..\..\Cadmus.Scripts\Modules
mklink /d SqlScripts ..\..\..\Cadmus.Scripts\SqlScripts
mklink Run.cmd ..\..\..\Cadmus.Scripts\Run.cmd
mklink Deploy.ps1 ..\..\..\Cadmus.Scripts\Deploy.ps1
mklink /d WebPackage ..\..\..\..\..\Nyx\Nyx.Web\Package
mklink /d DbUp ..\..\..\..\..\Nyx\Nyx.DbUp\bin\Debug
pause