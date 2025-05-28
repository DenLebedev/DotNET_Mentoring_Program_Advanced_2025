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
