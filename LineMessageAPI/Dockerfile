#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/runtime-deps:6.0.3-alpine3.15-arm64v8 AS base
#FROM mcr.microsoft.com/dotnet/runtime:6.0.3-focal-arm64v8 AS base
#FROM mcr.microsoft.com/dotnet/aspnet:6.0.3-alpine3.15-arm64v8 AS base
#FROM arm64v8/buildpack-deps:focal-curl AS base
FROM mcr.microsoft.com/dotnet/aspnet:6.0.3-focal-arm64v8 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#FROM mcr.microsoft.com/dotnet/aspnet:6.0.3-focal-arm64v8 AS build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#FROM mcr.microsoft.com/dotnet/nightly/sdk:6.0.3-focal-arm64v8 AS build
WORKDIR /src
COPY ["LineMessageAPI/LineMessageAPI.csproj", "LineMessageAPI/"]
RUN dotnet restore "LineMessageAPI/LineMessageAPI.csproj"
COPY . .
WORKDIR "/src/LineMessageAPI"
RUN dotnet build "LineMessageAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LineMessageAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LineMessageAPI.dll"]