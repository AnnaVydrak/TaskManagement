# TaskManagement Service

A simple task management microservice built with ASP.NET Core and PostgreSQL, designed to demonstrate message queuing with Azure Service Bus and RabbitMQ and a layered architecture for clean separation of concerns.

## Setup Instructions

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- PostgreSQL (can be run via Docker)
- RabbitMQ (also via Docker)/Azure Service Bus

## Build & Run locally (Docker Compose)
Use Docker Compose to start the service along with its dependencies:
docker-compose up --build

##  Design Overview

Architecture: Layered architecture (API, Application, Domain, Infrastructure).

Database: PostgreSQL with EF Core as ORM.

Message Broker: RabbitMQ is used for event-driven communication. And you can switch on implementation on Azure Service Bus, but you need use your Azure profile.

Dependency Injection: Built-in .NET DI container.

## Trade-offs & Limitations

RabbitMQ queue must be running and reachable for the service to operate correctly.

The current setup assumes a single environment (Development) for simplicity.

No built-in retry policies or circuit breakers for message handling—consider adding Polly or similar if needed.
