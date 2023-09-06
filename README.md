# NetFlow - Workflow Management Application

NetFlow is a .NET Core application designed to streamline and manage workflows efficiently. It provides a platform for designing, tracking, and optimizing workflows, making your business processes more organized and efficient.

## Features

- **Workflow Creation:** Easily create and customize workflows tailored to your organization's needs.
- **Workflow Instances:** Manage and monitor workflow instances with real-time status tracking.
- **Step Approvals:** Automate step approvals based on configurable time thresholds.
- **User-friendly Interface:** A user-friendly web interface for easy navigation and interaction.
- **Security:** Secure user authentication and authorization controls to protect sensitive data.

## Getting Started

Follow these steps to set up and run the NetFlow application on your local machine with a PostgreSQL database.

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) (3.1 or later)
- [PostgreSQL](https://www.postgresql.org/download/) database server

### Installation

1. Clone the repository:

```bash
git clone https://github.com/yourusername/netflow.git
```

2. Navigate to the project directory:

```bash
cd netflow
```

3. Configure `.env` file:

```plaintext
CONNECTION_STRING=""
ASPNETCORE_ENVIRONMENT="Development"
```

4. Install the required NuGet packages:

```bash
dotnet restore
```

5. Start database:

```bash
docker compose up -d
```

6. Apply database migrations to create the PostgreSQL database schema:

```bash
dotnet ef database update
```

7. Build and run the application:

```bash
dotnet build
dotnet run
```

8. Access the application in your web browser at http://localhost:5000/swagger.