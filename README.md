# Product Catalog Spring Financial

🛠 This backend is part of the Dynamic Product Catalog Filter project, built using a clean Domain-Driven Design (DDD) architecture with .NET 8 Web API, Entity Framework Core, and PostgreSQL.

🚀 Getting Started

1. 📦 Prerequisites
Visual Studio 2022
.NET 8 SDK
PostgreSQL with Docker

2. 🐳 Run PostgreSQL with Docker (Recommended)
Install Docker for Windows (or your preferred OS)

Create a docker-compose.yml at the root (it should have been commited already):

```
services:
    postgres:
        image: postgres:15
        container_name: productcatalog-db
        environment:
            POSTGRES_USER: postgres
            POSTGRES_PASSWORD: yourpassword
            POSTGRES_DB: ProductCatalogDb
        ports:
            - "5432:5432"
        volumes:
            - pgdata:/var/lib/postgresql/data
volumes:
    pgdata:
```

Start container:
```
docker compose up -d
```

3. 🔧 Configure Connection String
Edit ProductCatalog.API/appsettings.json:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=ProductCatalogDb;Username=postgres;Password=yourpassword"
  }
}
```

4. 🗃 Run EF Core Migration

Make sure the following file exists:
📄 ProductCatalog.Infrastructure/Data/CatalogDbContextFactory.cs

Then run:

# Apply migration
```
cd ProductCatalog
```

```
dotnet ef database update \
  --project ProductCatalog.Infrastructure \
  --startup-project ProductCatalog.API \
  --context CatalogDbContext
  ```

5. ▶ Run the API

```
dotnet run --project ProductCatalog.API
```
OR
In Visual Studio 2022, run ProductCatalog.API with https

Open Swagger UI:

https://localhost:7105/swagger

📬 API Endpoints
```
POST
/api/products/generate?count=1000
Generates fake products

GET
/api/products
Returns paginated list of products

GET
/api/products/search?q=term
Filters products dynamically
```

🧪 Run Tests
Unit tests are in ProductCatalog.Tests using EF InMemory:
```
cd ProductCatalog.Tests
dotnet test
```

💡 Notes
Follows clean architecture: API → Application → Domain + Infrastructure
DDD structure supports modular growth
Pagination supported in /products?page=1&pageSize=25

📚 Future Improvements
More validation, especially query string validation with regards to security
More unit tests - there is an issue with the Search unit test.

👨‍💻 Author
Xavier dela Cruz
