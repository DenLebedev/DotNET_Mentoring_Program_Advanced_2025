# DotNET_Mentoring_Program_Advanced_2025
.NET Mentoring Program Advanced [Asia Q1/Q2 2025]

Notes

[4/5/2025]

-   The CartingService project uses a NoSQL DB (LiteDB), the DB file is saved in the project root directory.

-   The CatalogService project uses a SQL DB (MySQL), the script (CatalogServiceDB_Create_Schema_Tables.sql) for creating the DB is located in the CatalogService\SQLScripts folder.

[4/10/2025]

-   AWS SQS is selected as the Message Broker.
    To test AWS SQS, you need to configure the following parameters in the AWS CLI or set environment variables:
    export AWS_ACCESS_KEY_ID= your-access-key // Provided upon request
    export AWS_SECRET_ACCESS_KEY=your-secret-key // Provided upon request
    export AWS_REGION=eu-central-1