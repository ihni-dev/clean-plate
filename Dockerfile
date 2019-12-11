FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/Adapters/Driving/CleanPlate.WebAPI/CleanPlate.WebAPI.csproj", "Adapters/Driving/CleanPlate.WebAPI/"]
RUN dotnet restore "Adapters/Driving/CleanPlate.WebAPI/CleanPlate.WebAPI.csproj"
COPY . .
WORKDIR "src/Adapters/Driving/CleanPlate.WebAPI"
RUN dotnet build "CleanPlate.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CleanPlate.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CleanPlate.WebAPI.dll"]