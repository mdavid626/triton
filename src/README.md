## Struktura

### Solution Nyx
Nyx je kódový název pro náš ukázkový projekt. Obsahuje následujúce části:

* Nyx.ClientTools: klientské nástroje aplikace. Webová aplikace bude s těmito nástroji komunikovat pomocí tzv. [URI helpers](https://msdn.microsoft.com/en-us/library/aa767914(v=vs.85).aspx)
* Nyx.ClientTools.Setup: WiX projekt k vytvoření MSI instalátoru klientských nástrojů
* Nyx.Cloud: cloudové nástroje, hlavně Microsoft Azure. Např. podpora [Application Insights](https://azure.microsoft.com/en-us/services/application-insights/)
* Nyx.DbProject: databázový projekt, obsahuje databázovou schému: tabulky, trigry, procedury atd.
* Nyx.DbUp: nástroj, pomocí kterého budeme aktualizovat databázové schéma, obsahuje databázové skripty
* Nyx.Doc: obsahuje dokumentace k projektu ve formátu .md (Markdown)
* Nyx.Environments: obsahuje konfigurační soubory prostředí. TP značí testovací prosředí, PP produkce.
* Nyx.Foundation: základní třídy, jakýsi framework
* Nyx.Reports: SQL reporty, hostované budou ve [Reporting Services Report Server](https://msdn.microsoft.com/en-us/library/ms157231.aspx)
* Nyx.Scheduler: nástroj, pomocí kterého je možné spustit naplánované úlohy
* Nyx.Web: projekt pro webovou aplikaci
* Nyx.Web.Tests: unit testy webové aplikace
* Nyx.Web.UiTestFramework: testovací framework pro UI testy
* Nyx.Web.UiTests: UI testy webové aplikace

### Solution Cadmus
Obsahuje nástroje podporující procesy Continous Delivery.

* Cadmus.DbUp: podpora aktualizací databázového schématu
* ...

### Solution Hector
Hector je Visual Studio extension, který obsahuje pomocné nástroje. Je to pomůcka, GUI pro command line aplikace.

* Hector: VSIX projekt
* ...

## Vývojové nástroje

* Visual Studio Enterprise 2015 
* Microsoft SQL Management Studio 2014
* JetBrains ReSharper Ultimate 10: dotCover, dotMemory, dotTrace, ReSharper
* Git
* Notepad++, Visual Studio Code
* Texmaker, MiKTeX, Pandoc
* PuTTY, PowerShell
* WiX Toolset
* CorelDRAW Graphics Suite X7
* ...

## Kódové názvy
viz Codenames.md