#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
RUN apt-get update && apt-get install wkhtmltopdf binutils -y && rm -rf /var/lib/apt/lists/*
RUN strip --remove-section=.note.ABI-tag /usr/lib/x86_64-linux-gnu/libQt5Core.so.5

WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ConversionTool/ConversionTool.csproj", "ConversionTool/"]
RUN dotnet restore "ConversionTool/ConversionTool.csproj"
COPY . .
WORKDIR "/src/ConversionTool"
RUN dotnet build "ConversionTool.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConversionTool.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ARG version=unknown
RUN echo $version > /app/wwwroot/version.txt
ENTRYPOINT ["dotnet", "ConversionTool.dll"]
