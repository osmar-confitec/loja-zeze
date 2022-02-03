#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["./apiGateways/ApiGatewayUser/ApiGatewayUser.csproj", "apiGateways/ApiGatewayUser/"]
COPY ["./buildingBlocks/BuildBlockServices/BuildBlockServices.csproj", "buildingBlocks/BuildBlockServices/"]
COPY ["./buildingBlocks/BuildBlockCore/BuildBlockCore.csproj", "buildingBlocks/BuildBlockCore/"]
RUN dotnet restore "apiGateways/ApiGatewayUser/ApiGatewayUser.csproj"
COPY . .
WORKDIR "apiGateways/ApiGatewayUser"
RUN dotnet build "ApiGatewayUser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApiGatewayUser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGatewayUser.dll"]