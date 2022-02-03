#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./services/CustomerApi/CustomerApi.csproj", "services/CustomerApi/"]
COPY ["./buildingBlocks/BuildBlockServices/BuildBlockServices.csproj", "buildingBlocks/BuildBlockServices/"]
COPY ["./buildingBlocks/BuildBlockCore/BuildBlockCore.csproj", "buildingBlocks/BuildBlockCore/"]
COPY ["./buildingBlocks/BuildBlockMessageBus/BuildBlockMessageBus.csproj", "buildingBlocks/BuildBlockMessageBus/"]

RUN dotnet restore "services/CustomerApi/CustomerApi.csproj"
COPY . .
WORKDIR "services/CustomerApi"
RUN dotnet build "CustomerApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomerApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomerApi.dll"]