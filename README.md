# Research Community Platform

![.NET 6.0](https://img.shields.io/badge/.NET-6.0-blueviolet) ![License: MIT](https://img.shields.io/badge/license-MIT-green)

## Overview
**Research Community Platform** is an open‑source web application that helps researchers organise publications, collaborate in communities, and showcase their work.  
Built with ASP.NET Core MVC and Tailwind CSS, it offers Google sign‑in, role‑based access control, and a SQL Server backend.

## Features
- **Authentication & Authorisation**
  - Cookie‑based and Google OAuth 2 sign‑in with email confirmation
  - Fine‑grained policies (`Create`, `Read`, `Update`, `Delete`)
- **Publication Management**
  - CRUD interface for papers with DOI and author list (stored as JSON)
  - File uploads and author‑to‑publication mapping
- **Community Spaces** (coming soon)
  - Group publications into themed communities
  - Role‑scoped discussion threads
- **Dashboards & Search**
  - Full‑text search over titles/DOIs
  - Personal dashboard for quick metrics
- **Developer‑friendly**
  - Clean service layer (`UserService`, `AuthorizationService`) and EF Core migrations
  - Tailwind‑powered UI with hot‑reload
  - Console and debug logging

## Tech Stack
| Layer        | Tech |
|--------------|------|
| **Backend**  | ASP.NET Core 6, EF Core, SQL Server |
| **Identity** | ASP.NET Core Identity, Google OAuth 2 |
| **Front‑End**| Razor Views, Tailwind CSS |
| **Tooling**  | Node 18+, `dotnet` CLI, Mailtrap (SMTP dev) |

## Getting Started

### Prerequisites
* [.NET 6 SDK](https://dotnet.microsoft.com/download)
* SQL Server LocalDB (or any SQL Server instance)
* Node.js ≥ 18 (for Tailwind build)
* Mailtrap account (or any SMTP sandbox)

### Setup

```bash
git clone https://github.com/ArmanShirzad/ResearchCommunityPlatform.git
cd ResearchCommunityPlatform

# 1. Update configuration
cp appsettings.Development.json.template appsettings.Development.json
#    - Set ConnectionStrings:MainConnection
#    - Put your Mailtrap credentials
#    - Add Google OAuth keys

# 2. Restore & build
dotnet restore
npm install
npm run css:build

# 3. Apply the database
dotnet ef database update   # uses included migrations

# 4. Run the application
dotnet run
```

Open your browser at `https://localhost:5001`.

### Seeding
On first run the app seeds demo roles (`Admin`, `Researcher`) and a sample user:  
`admin@example.com` / `P@ssw0rd!`

## Project Structure

```
📦ResearchCommunityPlatform
 ┣ 📂Controllers        # MVC + API controllers
 ┣ 📂Models             # Entity classes (User, Publication, ...)
 ┣ 📂Services
 ┃ ┣ 📂AuthorizationService
 ┃ ┗ 📂UserService
 ┣ 📂Views              # Razor views + partials
 ┣ 📂Migrations         # EF Core migrations
 ┣ 📂wwwroot            # Static assets compiled by Tailwind
 ┣ Program.cs           # Host & middleware setup
 ┗ tailwind.config.js
```

## Roadmap
- Community & Topic modules
- Real‑time notifications (SignalR)
- CI/CD with GitHub Actions
- Unit & integration test coverage

## Contributing

1. Fork the repo and create your branch (`git checkout -b feature/awesome`).
2. Commit your changes with clear messages.
3. Push to the branch.
4. Open a pull request detailing your changes.

All contributions, big or small, are welcome!

## License
This project is licensed under the **MIT License** – see [`LICENSE.txt`](LICENSE.txt) for details.
