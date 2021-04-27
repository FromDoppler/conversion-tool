#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update && apt-get install wkhtmltopdf -y && rm -rf /var/lib/apt/lists/*
RUN apt-get update && apt-get install -y \
    curl libxrender1 libfontconfig libxtst6 xz-utils
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["ConversionTool/ConversionTool.csproj", "ConversionTool/"]
RUN dotnet restore "ConversionTool/ConversionTool.csproj"
COPY . .
WORKDIR "/src/ConversionTool"
RUN dotnet build "ConversionTool.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConversionTool.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ConversionTool.dll"]
