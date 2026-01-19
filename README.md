# Store App

Simple store application with a REST API, using ASP.NET Core, Entity Framework Core, and Docker.

## Features

- REST API for managing products and ingredients
- Entity Framework Core for database access
- Docker for containerization
- Scalar/OpenAPI for API documentation

## Prerequisites

- .NET 9 SDK
- Docker

# Getting started

Start the database in docker:

```bash
docker compose up db
```

Start API in IDE.

## Database

Install the EF Core tools globally:

```bash
dotnet tool install --global dotnet-ef
```

Add the initial migration:

```bash
dotnet ef migrations add InitialCreate
```

Update the database:

```bash
dotnet ef database update
```

## Endpoints

### Products

| Method | Endpoint                                             | Description                    |
| ------ | ---------------------------------------------------- | ------------------------------ |
| GET    | /api/products                                        | Get all products               |
| GET    | /api/products/{id}                                   | Get product by ID              |
| POST   | /api/products                                        | Create a new product           |
| PUT    | /api/products/{id}                                   | Update a product               |
| DELETE | /api/products/{id}                                   | Delete a product               |
| POST   | /api/products/{productId}/ingredients/{ingredientId} | Add ingredient to product      |
| DELETE | /api/products/{productId}/ingredients/{ingredientId} | Remove ingredient from product |

### Ingredients

| Method | Endpoint                    | Description                         |
| ------ | --------------------------- | ----------------------------------- |
| GET    | /api/ingredients            | Get all ingredients                 |
| GET    | /api/ingredients/{id}       | Get ingredient by ID                |
| POST   | /api/ingredients            | Create a new ingredient             |
| PUT    | /api/ingredients/{id}       | Update an ingredient                |
| DELETE | /api/ingredients/{id}       | Delete an ingredient                |
| PATCH  | /api/ingredients/{id}/stock | Update ingredient stock status      |
| PATCH  | /api/ingredients/stock/bulk | Update all ingredients stock status |

| ------ | --------- | ----------------- |

## Project Structure

StoreApp/
├── Products/ # Product feature module
│ ├── controller/ # ProductsController
│ ├── Models/ # Product, ProductDto
│ └── service/ # IProductService, ProductsService
├── Ingredients/ # Ingredient feature module
│ ├── controller/ # IngredientsController
│ ├── Models/ # Ingredient, IngredientDto
│ └── service/ # IIngredientService, IngredientsService
├── Context/ # Database context
├── Exceptions/ # Custom exception types
├── Extensions/ # Database migration extensions
├── Profiles/ # AutoMapper profiles
└── Migrations/ # EF Core migrations
