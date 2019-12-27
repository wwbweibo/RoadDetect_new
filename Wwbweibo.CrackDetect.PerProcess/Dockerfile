#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Wwbweibo.CrackDetect.PerProcess/Wwbweibo.CrackDetect.PerProcess.csproj", "Wwbweibo.CrackDetect.PerProcess/"]
RUN dotnet restore "Wwbweibo.CrackDetect.PerProcess/Wwbweibo.CrackDetect.PerProcess.csproj"
COPY . .
WORKDIR "/src/Wwbweibo.CrackDetect.PerProcess"
RUN dotnet build "Wwbweibo.CrackDetect.PerProcess.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wwbweibo.CrackDetect.PerProcess.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wwbweibo.CrackDetect.PerProcess.dll"]