FROM microsoft/dotnet:1.1.0-sdk-projectjson-nanoserver
MAINTAINER Stuart Shay
LABEL version="1.0.1"

## Install NodeJS

ENV NPM_CONFIG_LOGLEVEL info  
ENV NODE_VERSION 6.9.2 
ENV NODE_SHA256 3525201f28c2298953c4e0b03fe4fb080bf295ec9a722af2abbaa4ad53d3b491

RUN powershell -Command \
   Invoke-WebRequest $('https://nodejs.org/dist/v{0}/node-v{0}-win-x64.zip' -f $env:NODE_VERSION) -OutFile 'node.zip' -UseBasicParsing ; \
   if ((Get-FileHash node.zip -Algorithm sha256).Hash -ne $env:NODE_SHA256) {exit 1} ; \
   Expand-Archive node.zip -DestinationPath C:\ ; \
   Rename-Item -Path $('C:\node-v{0}-win-x64' -f $env:NODE_VERSION) -NewName 'C:\nodejs' ; \
   New-Item $($env:APPDATA + '\npm') ; \
   $env:PATH = 'C:\nodejs;{0}\npm;{1}' -f $env:APPDATA, $env:PATH ; \
   Set-ItemProperty -Path 'HKLM:\SYSTEM\CurrentControlSet\Control\Session Manager\Environment\' -Name Path -Value $env:PATH ; \
   Remove-Item -Path node.zip

RUN npm install -g npm3

## Install Ruby (TODO) 
ENV RUBY_VERSION  2.2.4
ENV RUBY_SHA256 31203696adbfdda6f2874a2de31f7c5a1f3bcb6628f4d1a241de21b158cd5c76

RUN powershell -Command \
	$ErrorActionPreference = 'Stop'; \
	Invoke-WebRequest -Method Get -Uri http://dl.bintray.com/oneclick/rubyinstaller/rubyinstaller-%RUBY_VERSION%-x64.exe -OutFile c:\ruby.exe ; \
    # if ((Get-FileHash ruby.exe -Algorithm sha256).Hash -ne $env:RUBY_SHA256) {exit 1} ; \
	Start-Process c:\ruby.exe -ArgumentList '/verysilent' -Wait ; \
    Remove-Item c:\ruby.exe -Force

#RUN gem install compass --platform=ruby

## Copy SRC 
COPY src /app
WORKDIR /app

RUN dotnet restore

#WORKDIR /app/CoreDataStore.Web
RUN npm install
RUN npm run build
RUN dotnet build

EXPOSE 5000/tcp
#CMD [ "cmd" ]
#ENTRYPOINT ["dotnet", "CoreDataStore.Web.dll"]
ENTRYPOINT ["dotnet", "run"]