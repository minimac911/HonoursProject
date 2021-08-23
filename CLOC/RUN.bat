@ECHO OFF 
CD ../
ECHO ==========================
ECHO CREATE LINES OF CODE 
ECHO ==========================
cloc . --exclude-dir=wwwroot,Migrations,obj,bin,Properties,EventBus,.vs,.git,CLOC  --exclude-ext=csproj,csproj.user,dcproj,dcproj.user,sln,json --report-file=CLOC/output/LinesOfCode.log --by-file
ECHO ==========================
ECHO DONE
ECHO ==========================
CD CLOC/output/
LinesOfCode.log