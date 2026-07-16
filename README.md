# SoftOne Task Management Application

A task management app with a **.NET 8 Web API** backend and an **Angular 22** frontend (`SoftOneAPP`).

## Frontend HTTPS / HTTP

In SoftOneAPP `angular.json` under `serve.options`:

```json
"builder": "@angular/build:dev-server",
"options": {
  "ssl": false
}
```

Use `"ssl": true` to run the frontend with HTTPS, and set the API base URL in SoftOneAPP `src/environments/environment.ts` (for example IIS Express `https://localhost:44326/api`).

If HTTPS has certificate issues, keep `"ssl": false` and run the backend over HTTP with the same host/port in `environment.ts`.

## Default credentials

| Username | Password |
|----------|----------|
| `admin`  | `admin123` |
| `demo`   | `demo123` |


|----Database----|


LocalDB connection string is in `SoftOne/appsettings.json`. Migrations live under `SoftOne/Migrations`; SQL script under `/dataBase_Script.sql`.
Please use below script to create db with data 
SoftOne/Migrations/dataBase_Script.sql
|----Database----|

## Run API

```bash
dotnet run --project SoftOne --launch-profile http
```

Swagger: [http://localhost:5030/swagger](http://localhost:5030/swagger)

## Run Angular UI

```bash
cd ../SoftOneAPP
npm install
npm start
```

## UI styling (assignment optional)

SoftOneAPP includes basic styling without a UI framework:

- Shared CSS variables and Inter font
- Login card with gradient background
- Side-by-side task list and add/update form
- Status badges, priority colors, completed strikethrough
- Loading spinner, empty state, animated toasts
- Responsive layout for smaller screens

See [SoftOneAPP/README.md](../SoftOneAPP/README.md) for UI details and where to add screenshots for GitHub.

## Database

## Tests

```bash
dotnet test SoftOne.sln
```
