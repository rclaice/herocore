# herocore
.net core 2.1 + angular application using the angular.io tour of heroes tutorial as the base site

This reposotory contains the base assets with with tour of heroes very close to what you see when you complete the tutorial from the [Angular IO Tutorial](https://angular.io/tutorial)

## I build this application using [VS Code](https://code.visualstudio.com/)
1. Install Visual Studio Code (make sure you can create and run a .net core application built with c#) for more info [Start Here](https://code.visualstudio.com/docs/other/dotnet)
2. Install latest SPA templates from vs code terminal
```
dotnet new --install Microsoft.DotNet.Web.Spa.ProjectTemplates::2.1.0 
dotnet new --install Microsoft.AspNetCore.SpaTemplates::2.1.0-preview1-final 
```
3. Clone/Fork this repository
4. Open the herocore/coreheros directory in VS Code
5. From Terminal navigate to the ClientApp folder 
6. run 
```
ng update @angular/cli 
npm install
```
7. F5 and the application should run 

Open vs code, navigate to folder
git clone https://github.com/rclaice/herocore.git
cd herocore
git checkout working
cd ..cd

from herocore/coreheroes/ClientApp
```
 1. ng update @angular/cli
 2.  npm install 
```
The [step by step](herocore/parts/stepbystep.pptx) has high level steps used during the presentation the [parts](herocore/parts) folder contains the assets used.



