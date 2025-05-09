# ERP Project - Clean Architecture

## Overview

This is an ERP (Enterprise Resource Planning) system built using **Clean Architecture** principles with **ASP.NET Core**. The project aims to provide a modular, maintainable, and scalable application with separation of concerns and clear boundaries between different layers.

## Features

- **Modular Design**: The system is designed with a clean separation of concerns using the Clean Architecture principles.
- **Entity Framework Core**: ORM for database interactions.
- **Dependency Injection**: Built-in dependency injection in ASP.NET Core to manage service dependencies.
- **Automated Testing**: Includes unit tests to ensure code quality and system reliability.
- **Background Tasks**: Use of Hangfire or similar to manage background tasks.

## Architecture

The project follows **Clean Architecture**, which divides the system into several layers:

1. **Core Layer**:
    - Contains **Entities**, **Value Objects**, **Aggregates**, **Interfaces** for repositories, and other business logic.
    
2. **Application Layer**:
    - Includes **Use Cases**, **DTOs**, **Services**, and **Interfaces** for external service interactions (e.g., email, payment gateways).

3. **Infrastructure Layer**:
    - Implements **Repositories**, **EF Core DbContext**, **External APIs**, **File Storage**, and other infrastructural concerns.
    
4. **Web API Layer**:
    - **Controllers** to handle incoming HTTP requests and communicate with the Application Layer.
    - **API versioning**, **Swagger documentation**, and **Authentication & Authorization**.

## Project Structure

