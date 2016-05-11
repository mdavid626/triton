@echo off
setlocal

cd bin\debug
mklink /d Modules ..\..\..\Cadmus.Scripts\Modules
mklink /d SqlScripts ..\..\..\Cadmus.Scripts\SqlScripts
mklink /d PsScripts ..\..\..\Cadmus.Scripts\PsScripts
mklink /d Cookbooks ..\..\..\Cadmus.Scripts\Cookbooks
mklink Run.cmd ..\..\..\Cadmus.Scripts\Run.cmd
mklink Deploy.ps1 ..\..\..\Cadmus.Scripts\Deploy.ps1
mklink vagrantfile ..\..\..\Cadmus.Scripts\vagrantfile
mklink app_offline.htm ..\..\..\Cadmus.Scripts\app_offline.htm
mklink setup.msi ..\..\..\..\..\Nyx\Nyx.ClientTools.Setup\bin\Debug\setup.msi
mklink cab1.cab ..\..\..\..\..\Nyx\Nyx.ClientTools.Setup\bin\Debug\cab1.cab
mklink /d WebPackage ..\..\..\..\..\Nyx\Nyx.Web\Package
mklink /d Scheduler ..\..\..\..\..\Nyx\Nyx.Scheduler\bin\debug
mklink /d DbUp ..\..\..\..\..\Nyx\Nyx.DbUp\bin\Debug
mklink /d Report ..\..\..\..\..\Nyx\Nyx.Reporting
pause