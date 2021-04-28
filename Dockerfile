FROM node:16 AS verify-format
WORKDIR /src
COPY package.json yarn.lock ./
RUN yarn
COPY . .
RUN yarn verify-format

FROM koalaman/shellcheck-alpine as verify-sh
WORKDIR /src
COPY ./*.sh ./
RUN shellcheck -e SC1091,SC1090 ./*.sh

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS restore
WORKDIR /src
COPY ./*.sln ./
COPY */*.csproj ./
COPY ./.config/dotnet-tools.json ./.config/
# Take into account using the same name for the folder and the .csproj and only one folder level
RUN for file in $(ls *.csproj); do mkdir -p ${file%.*}/ && mv $file ${file%.*}/; done
RUN dotnet tool restore
RUN dotnet restore

FROM restore AS build
COPY . .
RUN dotnet dotnet-format --check
RUN dotnet build -c Release

FROM build AS test
RUN apt-get update && apt-get install wkhtmltopdf -y && rm -rf /var/lib/apt/lists/*
RUN dotnet test

FROM build AS publish
RUN dotnet publish "./ConversionTool/ConversionTool.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS final
RUN apt-get update && apt-get install wkhtmltopdf binutils -y && rm -rf /var/lib/apt/lists/*
RUN strip --remove-section=.note.ABI-tag /usr/lib/x86_64-linux-gnu/libQt5Core.so.5
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ARG version=unknown
RUN echo $version > /app/wwwroot/version.txt
ENTRYPOINT ["dotnet", "ConversionTool.dll"]
