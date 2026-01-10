# Cancun Booking API - Setup Guide

## Prerequisites
- .NET 8 SDK
- MySQL Database
- Docker (optional)

## Configuration

### Environment Variables
Set the following environment variables or update `appsettings.json`:

```bash
DB_SERVER=localhost
DB_DATABASE=cancun_db
DB_USER=root
DB_PASSWORD=password
```

See `.env.example` for reference.

## Running the Application

### Option 1: Using .NET CLI
```bash
cd src/API
dotnet run
```

The API will be available at `http://localhost:5000`

### Option 2: Using Docker
```bash
docker build -t cancun-api .
docker run -p 8080:8080 \
  -e DB_SERVER=host.docker.internal \
  -e DB_DATABASE=cancun_db \
  -e DB_USER=root \
  -e DB_PASSWORD=password \
  cancun-api
```

The API will be available at `http://localhost:8080`

## API Endpoints

### Swagger Documentation
Access the interactive API documentation at: `http://localhost:5000/swagger` (or port 8080 for Docker)

### POST /ReservationOrders
Create a new reservation.

**Request Body:**
```json
{
  "startDate": "2026-01-15T00:00:00",
  "endDate": "2026-01-17T00:00:00",
  "customerEmail": "customer@example.com"
}
```

**Response:** 201 Created

### PUT /ReservationOrders
Update an existing reservation.

**Request Body:**
```json
{
  "id": 1,
  "startDate": "2026-01-16T00:00:00",
  "endDate": "2026-01-18T00:00:00",
  "customerEmail": "customer@example.com"
}
```

**Response:** 204 No Content

### DELETE /ReservationOrders
Cancel a reservation.

**Headers:**
- `X-Reservation-Id`: Reservation ID
- `X-Customer-Email`: Customer email

**Response:** 204 No Content

## Booking Rules

- **Maximum stay**: 3 days
- **Maximum booking advance**: 30 days
- **Minimum booking advance**: 1 day (cannot book for same day)

## Running Tests

```bash
cd src
dotnet test
```

## Database Setup

The application will automatically create the database schema on first run using Entity Framework Core migrations.

To manually create the database:
```bash
cd src/API
dotnet ef database update
```

## Project Structure

```
src/
├── API/                    # Web API project
│   ├── Controllers/        # API controllers
│   ├── Extensions/         # IoC extensions
│   └── Program.cs          # Application entry point
├── Application/            # Business logic layer
│   ├── Dto/                # Data transfer objects
│   ├── Services/           # Business services
│   └── Interfaces/         # Service interfaces
├── Domain/                 # Domain entities
│   ├── Entities/           # Domain models
│   └── Enuns/              # Enumerations
├── Infra/                  # Infrastructure layer
│   ├── Configurations/     # EF configurations
│   └── CancunDbContext.cs  # Database context
└── API.Tests/              # Unit tests
```

## Technology Stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- xUnit (testing)
- Swagger/OpenAPI
