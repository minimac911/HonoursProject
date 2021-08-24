RUN COMMAND IN ROOT OF PROJECT

cloc . --exclude-dir=wwwroot,Migrations,obj,bin,Properties,EventBus,.vs,.git,CLOC,_MySQL_Scripts  --exclude-ext=csproj,csproj.user,dcproj,dcproj.user,sln,json --report-file=CLOC/output/LinesOfCode.txt --by-file

https://github.com/AlDanial/cloc

Install CLOC:
	choco install clo

EXPLANATION
Directories ignored:
	/wwwroot/
	/Migrations/
	/obj/
	/bin/
	/Properties/
	/EventBus/
	/.vs/
	/.git/
	/CLOC/
	/_MySQL_Scripts/
File extensions ignored:
	.csproj
	.csproj.user
	.dcproj
	.dcproj.user
	.sln
	.json