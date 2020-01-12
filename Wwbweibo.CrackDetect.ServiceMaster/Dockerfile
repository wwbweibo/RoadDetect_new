#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Wwbweibo.CrackDetect.ServiceMaster/Wwbweibo.CrackDetect.ServiceMaster.csproj", "Wwbweibo.CrackDetect.ServiceMaster/"]
RUN dotnet restore "Wwbweibo.CrackDetect.ServiceMaster/Wwbweibo.CrackDetect.ServiceMaster.csproj"
COPY . .
WORKDIR "/src/Wwbweibo.CrackDetect.ServiceMaster"
RUN dotnet build "Wwbweibo.CrackDetect.ServiceMaster.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Wwbweibo.CrackDetect.ServiceMaster.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Wwbweibo.CrackDetect.ServiceMaster.dll"]