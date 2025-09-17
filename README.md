# 🚀 .NET 8 CQRS & Event-Driven Order API

> A clean, minimal, and production-ready sample that demonstrates **CQRS** with an **event-driven read model** using .NET 8, EF Core and SQLite.

[![.NET](https://img.shields.io/badge/.NET-8-blue)](#) [![License](https://img.shields.io/badge/license-MIT-green)](#) [![Status](https://img.shields.io/badge/status-experimental-orange)](#)

---

## ✨ Project Overview

This repository implements a small but realistic Orders API that separates **writes** (commands) from **reads** (queries). The write side persists full domain entities and publishes events. Projection handlers consume those events to keep a lightweight **read model** (optimized for queries) in sync.

It’s intentionally minimal so you can read, learn, and extend the architecture for production usage patterns (event buses, snapshots, distributed transactions, etc.).

---

## 🔍 Table of contents

1. [Architecture & Flow](#-architecture--flow)  
2. [Key Components](#-key-components)  
3. [Tech Stack](#-tech-stack)  
4. [Getting started](#-getting-started)  

---

## 🧭 Architecture & Flow

**High level:** commands → write model → events → projections → read model → queries

Client --> HTTP API --> Command (CreateOrderCommand)
│
└─> WriteDbContext (persist Order)
└─> Publish(OrderCreatedEvent)
└─> Projection handler (OrderCreatedProjectionHandler)
└─> ReadDbContext (create OrderDto)

Client --> HTTP API --> Query (GetOrdersListQuery) --> ReadDbContext --> DTO

markdown
Copy code

**Why this structure?**  
- Clear separation of responsibilities.  
- Read model is optimized for fast queries and can evolve independently.  
- Events act as the source of truth for eventual consistency between models.  

---

## 🧩 Key Components

- **Commands & Handlers** (`Features/Commands/`)  
  - `CreateOrderCommand`, `CreateOrderCommandHandler`, `CreateOrderCommandValidator` (FluentValidation)  
- **Queries & Handlers** (`Features/Queries/`)  
  - `GetOrdersListQuery`, `GetOrderByIdQuery`, and their handlers that read from `ReadDbContext`  
- **Events & Publishers** (`Services/Events/`)  
  - `IEventPublisher`, `InProcessEventPublisher`, `ConsoleEventPublisher`  
- **Projections** (`Projections/`)  
  - `OrderCreatedProjectionHandler` listens for `OrderCreatedEvent` and updates `ReadDbContext`  
- **Data Layer** (`Data/`)  
  - `WriteDbContext` and `ReadDbContext` (EF Core + SQLite)  
- **DTOs** (`Dtos/`) — lightweight objects returned by API endpoints  
- **Migrations** — separated per DB (`Migrations/Read`, `Migrations/Write`)  

---

## 🛠 Tech Stack

- **.NET 8 / ASP.NET Core**  
- **Entity Framework Core (EF Core)**  
- **SQLite** (file-based `write.db` and `read.db` under `Data/DB`)  
- **MediatR** or similar mediator pattern (if used) for command/query dispatching  
- **FluentValidation** for command validation  
- **Swagger / OpenAPI** for interactive documentation  

---

## 🚀 Getting started (quick)

> Tested with .NET 8 SDK. Replace `YourProjectName` with your project file if needed.


# clone
git clone <your-repo-url>
cd <repo-folder>

# restore
dotnet restore

# apply migrations (Write + Read)
dotnet ef database update --context WriteDbContext --project Data --startup-project .

dotnet ef database update --context ReadDbContext --project Data --startup-project .

# run the app
dotnet run --project .
Open: https://localhost:5001/swagger to explore endpoints.
