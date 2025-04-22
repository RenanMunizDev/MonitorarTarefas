# MonitorarTarefas API

API RESTful para gerenciamento de projetos e tarefas, com foco em produtividade, organização e colaboração.

---

## 🚀 Tecnologias

- .NET 8
- Entity Framework Core
- SQL Server (Docker)
- AutoMapper
- Swagger (OpenAPI)
- xUnit (testes)

---

## 📦 Como executar localmente com Docker

### Pré-requisitos

- [.NET SDK 8+](https://dotnet.microsoft.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

---

### 🔧 Rodar o SQL Server no Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=MinhaSenha123!" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
