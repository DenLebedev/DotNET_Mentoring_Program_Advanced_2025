# DotNET_Mentoring_Program_Advanced_2025

## Project Overview

This solution contains three primary services implemented in ASP.NET Core (.NET 8):

- `IdentityService` – Handles user authentication, token issuance (access + refresh tokens), and role management using Duende IdentityServer.
- `CatalogService` – A secured service with role-based access. Only users with the `Manager` role can modify catalog data.
- `CartingService` – A secured service accessible to both `Manager` and `StoreCustomer` roles. It logs token details via custom middleware.

## Notes

### [4/5/2025]
-   The CartingService project uses a NoSQL DB (LiteDB), the DB file is saved in the project root directory.
-   The CatalogService project uses a SQL DB (MySQL), the script (CatalogServiceDB_Create_Schema_Tables.sql) for creating the DB is located in the CatalogService\SQLScripts folder.


### [4/10/2025]
-   AWS SQS is selected as the Message Broker.
    To test AWS SQS, you need to configure the following parameters in the AWS CLI or set environment variables:
    export AWS_ACCESS_KEY_ID= your-access-key // Provided upon request
    export AWS_SECRET_ACCESS_KEY=your-secret-key // Provided upon request
    export AWS_REGION=eu-central-1

### [4/13/2025]

#### User Credentials and Roles

| Username              | Password       | Role          | Permissions                   |
|-----------------------|----------------|---------------|-------------------------------|
| manager@test.com      | Manager123$    | Manager       | read, create, update, delete  |
| customer@test.com     | Customer123$   | StoreCustomer | read                          |

---

#### Testing Instructions

##### 1. Start All Services

Make sure the following services are running:

- IdentityService: `https://localhost:7051`
- CatalogService: `https://localhost:7052`
- CartingService: `https://localhost:7053`

##### 2. Get a Token

**Using curl:**

```
curl -k -X POST https://localhost:7051/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password&client_id=swagger-ui&client_secret=swagger-secret&username=manager@test.com&password=Manager123$&scope=openid profile catalog_api carting_api offline_access"
```

Copy the `access_token` and use it in Swagger or Postman.

---

##### 3. Refresh Token

```
curl -k -X POST https://localhost:7051/connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=refresh_token&client_id=swagger-ui&client_secret=swagger-secret&refresh_token=YOUR_REFRESH_TOKEN"
```

---

#### Access Control

| Service        | Endpoint Access              | Authorized Roles       |
|----------------|-------------------------------|-------------------------|
| CatalogService | `GET` (public)                | All users               |
| CatalogService | `POST/PUT/DELETE`             | Manager only            |
| CartingService | All endpoints                 | Manager, StoreCustomer  |

---

#### Token Logging

CartingService logs the following info for each request:

- Subject (`sub`)
- Roles
- Permissions

Console output example:

```
Access Token Details:
 - Subject (sub): a12bc34d...
 - Roles: StoreCustomer
 - Permissions: read
```

### [4/17/2025]

#### Containerization and Orchestration

The solution now supports full containerization using Docker and Docker Compose.

##### Docker Compose Setup

A docker-compose.yml file has been added at the root level. It defines the following services:
- mysql: MySQL container for CatalogService
- elasticmq: Local SQS simulator for message queue testing
- identityservice: IdentityServer for user authentication
- catalogservice: Catalog microservice
- cartingservice: Cart microservice

##### Instructions
1. Build and start all containers:
```
docker-compose up --buildTest in Swagger:
```
2. Test in Swagger:
Navigate to:
- IdentityService: http://localhost:7051/swagger
- CatalogService: http://localhost:8080/swagger
- CartingService: http://localhost:7053/swaggers

##### Volumes
- Database and LiteDB data is persisted in local Docker volumes

##### File Locations
- ./scripts/ - SQL schema
- docker-compose.yml - Compose orchestration
- appsettings.Docker.json - Used for Docker environment configuration

### [5/23/2025]

#### API Gateway Integration
- API Gateway is implemented using Ocelot.
- GatewayService listens on https://localhost:7050 and serves as the entry point for client requests.
- It forwards incoming requests to either CatalogService or CartingService based on configured routing rules.
- Authentication is enforced at the gateway level by validating JWT tokens issued by IdentityService.
- The gateway configuration is defined in GatewayService/ocelot.json.
- The setup supports local development only and does not include Docker or containerization.
- Swagger UI is available at https://localhost:7050/swagger to explore API endpoints through the gateway.

### [5/29/2025]
-   **Advanced Logging and Tracing** has been integrated across all services.

#### Logging with Serilog
- Serilog is configured in all services (`CatalogService`, `CartingService`, `APIGateway`, `IdentityService`).
- Logs are output to both Console and Azure Application Insights (using `Serilog.Sinks.ApplicationInsights`).
- All logs include a structured `CorrelationId` for traceability.

#### Correlation ID Middleware
- Middleware automatically handles `X-Correlation-ID` header:
  - Generates a new Correlation ID if missing
  - Adds the ID to the response header
  - Stores the ID in `HttpContext.Items` for use in controllers
  - Adds the ID to `Activity.Baggage` and `Serilog.LogContext` for full trace propagation and log enrichment

#### Distributed Tracing
- Application Insights SDK automatically tracks incoming requests and HTTP dependencies.

#### Observability in Azure
- Azure Application Insights is configured for all services.
- End-to-end transactions can be visualized using `Transaction Search` or `Application Map` in the Azure Portal.

### [6/2/2025]
-   **GraphQL API** 

#### GraphQL Integration in CatalogService
- CatalogService now supports GraphQL via HotChocolate.
- The GraphQL endpoint is available at: https://localhost:7052/graphql
- Authorization is enforced via role-based access:
  - Only Manager users can create, update, or delete resources.
  - All users can query public data.

#### Available GraphQL Operations
- Queries
```
# Get all categories
query {
  categories {
    id
    name
    imageUrl
    parentCategoryId
    products {
      id
      name
      price
    }
  }
}

# Get all products (with filters and sorting)
query {
  products(where: { price: { gt: 50 } }, order: { price: DESC }) {
    id
    name
    description
    price
    category {
      id
      name
    }
  }
}

# Get single category by ID
query {
  category(id: 1) {
    id
    name
    products {
      name
      price
    }
  }
}
```
- Mutations
```
# Add new category (Manager only)
mutation {
  addCategory(input: {
    name: "Books"
    imageUrl: "https://example.com/book.png"
    parentCategoryId: null
  }) {
    id
    name
  }
}

# Update existing product (Manager only)
mutation {
  updateProduct(id: 1, input: {
    name: "Updated Product Name"
    description: "Updated description"
    imageUrl: "https://example.com/updated.png"
    price: 199.99
    amount: 10
    categoryId: 2
  }) {
    id
    name
    price
  }
}

# Delete product (Manager only)
mutation {
  deleteProduct(id: 3)
}
```

#### Optimized Data Access
- N+1 query prevention is implemented using DataLoader for batch-fetching related data (e.g., Products by Category).
- Performance improvements are visible in nested queries (e.g., querying products under categories

#### Notes for Testing
- Test GraphQL using [Banana Cake Pop](https://chillicream.com/docs/bananacakepop) or Postman with the appropriate JWT bearer token.
- Ensure you include the Authorization header:
```
Authorization: Bearer {access_token}
```