# 🧩 MonitorarTarefas API

![.NET](https://img.shields.io/badge/.NET-8.0-blue)
![Docker](https://img.shields.io/badge/Docker-Ready-blue)
![Tests](https://img.shields.io/badge/Testes-80%25-green)
![License](https://img.shields.io/badge/license-MIT-lightgrey)

API RESTful para controle de projetos e tarefas, com histórico de alterações, comentários e relatórios de desempenho.  
Ideal para acompanhar produtividade de equipes e organizar demandas com clareza.

---

## 🚀 Tecnologias

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (Docker)
- AutoMapper
- Swagger (OpenAPI)
- xUnit (Testes)

---

## ⚙️ Execução Local com Docker

### 📦 Requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

### 🐳 Subir SQL Server via Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MinhaSenha123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
