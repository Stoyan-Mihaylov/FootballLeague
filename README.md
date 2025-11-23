# FootballLeague
FootballLeague is a C#/.NET application for managing football teams, matches, and rankings. It provides services for creating, updating, and deleting teams and matches, calculating points, and retrieving team rankings.

## Features:
- Manage football teams: create, update, delete.
- Manage matches: schedule, update, and delete matches.
- Compute team rankings based on match results.
- Tracks points, wins, draws, losses, and goals scored/conceded.
- Fully unit tested with NUnit and Moq.

## Project Structure:
- **FootballLeague.Api** - API project exposing endpoints for teams, matches, and rankings.
- **FootballLeague.Application** - Application layer with services and business logic.
- **FootballLeague.Domain** - Domain entities and enums.
- **FootballLeague.Infrastructure** - Data access layer using Entity Framework Core.
- **FootballLeague.Application.Tests** - Unit tests for application services.

## Prerequisites:
- Before running the project, ensure you have:
- [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or modify the connection string for another database)
- IDE like [Visual Studio 2022](https://visualstudio.microsoft.com/) or [JetBrains Rider](https://www.jetbrains.com/rider/)

### Getting Started:

### 1. Clone the Repository:
- git clone https://github.com/Stoyan-Mihaylov/FootballLeague.git
- cd FootballLeague

### 2. Apply Database Migrations:
- Run the following commands in the terminal from the FootballLeague.Infrastructure project directory:
- dotnet ef database update
- This will create the database schema and required tables.

### 3. Run the API:
- Navigate to the API project directory and run the application:
- cd FootballLeague.Api
- dotnet run

By default, the API will be available at:
- https://localhost:5001
- http://localhost:5000

### 4. Running Unit Tests:
- Unit tests are located in the FootballLeague.Application.Tests project. Run them with:
- cd FootballLeague.Application.Tests
- dotnet test
