@echo off
setlocal

cd bin\debug
mklink /d Modules ..\..\..\Cadmus.Scripts\Modules
mklink Deploy.ps1 ..\..\..\Cadmus.Scripts\Deploy.ps1
mklink Run.ps1 ..\..\..\Cadmus.Scripts\Run.ps1
pause