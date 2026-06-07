FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TaskManager.Api/TaskManager.Api.csproj TaskManager.Api/
RUN dotnet restore TaskManager.Api/TaskManager.Api.csproj

COPY . .
RUN dotnet publish TaskManager.Api/TaskManager.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "TaskManager.Api.dll"]
