#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Wwbweibo.CrackDetect.MasterWeb/Wwbweibo.CrackDetect.MasterWeb.csproj", "Wwbweibo.CrackDetect.MasterWeb/"]
RUN dotnet restore "Wwbweibo.CrackDetect.MasterWeb/Wwbweibo.CrackDetect.MasterWeb.csproj"
COPY . .
WORKDIR "/src/Wwbweibo.CrackDetect.MasterWeb"
RUN dotnet build "Wwbweibo.CrackDetect.MasterWeb.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wwbweibo.CrackDetect.MasterWeb.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wwbweibo.CrackDetect.MasterWeb.dll"]