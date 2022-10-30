#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BusinessCards.Api/BusinessCards.Api.csproj", "BusinessCards.Api/"]
COPY ["BusinessCards.Infrastructure/BusinessCards.Infrastructure.csproj", "BusinessCards.Infrastructure/"]
COPY ["BusinessCards.Domain/BusinessCards.Domain.csproj", "BusinessCards.Domain/"]
RUN dotnet restore "BusinessCards.Api/BusinessCards.Api.csproj"
COPY . .
WORKDIR "/src/BusinessCards.Api"
RUN dotnet build "BusinessCards.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BusinessCards.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BusinessCards.Api.dll"]