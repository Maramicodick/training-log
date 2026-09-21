# Training log

Personal completed-activity log. API is unauthenticated on localhost.

## Run in development

Terminal 1:

```
dotnet run --project TrackingApp.Backend --launch-profile http
```

Terminal 2:

```
cd TrackingApp.Frontend
npm install
npm run dev
```

Open the Vite URL (usually http://localhost:5173). `/api` is proxied to http://127.0.0.1:5247.

## Combined host (API + built UI)

```
cd TrackingApp.Frontend
npm run build
dotnet run --project TrackingApp.Backend --launch-profile http
```

Open http://localhost:5247. `npm run build` writes into `TrackingApp.Backend/wwwroot`.

## Database

SQL Server LocalDB. Connection string is in `TrackingApp.Backend/appsettings.json`. Default files: `E:\Database\TrackingApp.mdf`.
