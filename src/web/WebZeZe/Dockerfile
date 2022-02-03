#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./web/WebZeZe/WebZeZe.csproj", "web/WebZeZe/"]
COPY ["./buildingBlocks/BuildBlockServices/BuildBlockServices.csproj", "buildingBlocks/BuildBlockServices/"]
COPY ["./buildingBlocks/BuildBlockCore/BuildBlockCore.csproj", "buildingBlocks/BuildBlockCore/"]
RUN dotnet restore "web/WebZeZe/WebZeZe.csproj"
COPY . .
WORKDIR "web/WebZeZe"
RUN dotnet build "WebZeZe.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebZeZe.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebZeZe.dll"]