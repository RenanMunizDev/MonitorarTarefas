# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY MonitorarTarefas.API/*.csproj MonitorarTarefas.API/
COPY MonitorarTarefas.Domain/*.csproj MonitorarTarefas.Domain/
COPY MonitorarTarefas.Infrastructure/*.csproj MonitorarTarefas.Infrastructure/

RUN dotnet restore

COPY . .
WORKDIR /src/MonitorarTarefas.API
RUN dotnet publish -c Release -o /app/publish

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MonitorarTarefas.API.dll"]
