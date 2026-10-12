# MES Dashboard – Radial Grader

A web-based MES dashboard built to monitor machine status, production progress, batch details, and output performance from a single screen.

The project connects machine data with production information to provide a clear view of ongoing manufacturing operations.

## Dashboard Preview

<img width="1920" height="1032" alt="image" src="https://github.com/user-attachments/assets/0d70130b-66a4-463a-9da3-48e1330d0396" />


![MES Dashboard](docs/mes-dashboard.png)

## Features

- Live machine status and speed monitoring
- PLC connection status and last update time
- Production order and active batch tracking
- Target quantity and production progress
- Input weight, output weight, good production, and reject tracking
- Grade-wise output visualization
- Production output trends using charts
- Machine health, downtime, OEE, temperature, and production rate
- Database-backed production and machine information

## Technology Stack

- **Backend:** C#, ASP.NET Core MVC, Web API
- **Real-Time Updates:** SignalR
- **Database:** PostgreSQL
- **Frontend:** HTML, CSS, JavaScript, Bootstrap
- **Charts:** Chart.js
- **Industrial Integration:** PLC communication and machine data acquisition

## How It Works

The application collects machine data through the PLC integration layer, processes it in the backend, and displays machine and production information on the MES dashboard.

**Data flow:** PLC / Machine → Backend Application → Database → MES Dashboard

## Run Locally

### Requirements

- .NET SDK compatible with the project
- PostgreSQL
- Visual Studio 2022 or Visual Studio Code
- PLC connection or simulator, depending on your configuration

### Steps

1. Clone this repository:

   ```bash
[   git clone <your-repository-url>](https://github.com/niteesh017/MES/)
   ```

2. Open the solution in Visual Studio or navigate to the project folder.

3. Configure the PostgreSQL connection string in `appsettings.json`.

4. Configure the PLC or simulator endpoint.

5. Restore the dependencies:

   ```bash
   dotnet restore
   ```

6. Run the application:

   ```bash
   dotnet run
   ```

7. Open the local URL shown in the terminal.

## Project Purpose

This project demonstrates application development for manufacturing environments, combining .NET development, database integration, PLC connectivity, and production monitoring.

## Author

**Niteesh Kumar**

Application Developer | .NET | MES & Industrial Automation
