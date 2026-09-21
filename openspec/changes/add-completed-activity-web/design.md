## Context

See `proposal.md` for motivation. The repo has no application code yet. v1 is a PC web app; a MAUI phone client is explicitly later and must consume the same HTTP API rather than a second data store.

## Goals / Non-Goals

**Goals:**

- ASP.NET Core JSON API as the only backend; Vue 3 SPA as the PC UI.
- One ASP.NET Core project for the API and persistence. Split out shared C# libraries only if MAUI later needs them.
- In production, one process: the API host serves the built Vue files.
- SQL Server persistence (connection string in config) so the same engine can stay when the app grows.

**Non-Goals:**

- MAUI project, iOS/Android packaging, or a public hosted multi-tenant service.
- Authentication, accounts, or per-user isolation.
- Sharing UI between web and MAUI (Vue vs native UI stay separate).
- Planned workouts, calendar views, or expected-vs-actual fields.

## Decisions

### 1. Web first, MAUI later against the same API

Ship a Vue SPA in the browser for PC. Do not add a MAUI head in this change. Phone can wait; the API is the seam.

**Alternatives considered:** MAUI-only (fails "use on PC in a browser"); web + MAUI in one change (too large); Blazor for the web UI (rejected in favor of Vue).

### 2. Vue 3 + Vite + TypeScript for the web UI

The browser app is a Vue 3 SPA. It calls `GET/POST /api/activities` like MAUI will later. Validation is enforced on the API; the Vue form mirrors those rules for UX (non-empty title, date required, default date today) but is not the source of truth.

Dev: Vite dev server with a proxy to the API. Prod: `npm run build` output copied/served from the ASP.NET host `wwwroot`.

**Alternatives considered:** Blazor Server (one language, but a live circuit instead of a normal SPA; user chose Vue); React (equivalent SPA; user chose Vue); Angular (heavier than this form needs).

### 3. JSON API shape

- `GET /api/activities` — list, date descending, then most recently added.
- `POST /api/activities` — create `{ date, title, notes? }`; `400` on validation failure with field errors.
- Each activity has a server-generated id (GUID) for later edit/delete without putting those operations in v1 UI.

Keep the path and payload stable so MAUI can call it unchanged.

**Alternatives considered:** OData/GraphQL (overkill); gRPC (awkward for a future simple mobile client and browser).

### 4. Solution layout

```
TrackingApp.sln
TrackingApp.Backend/              ASP.NET Core API + SQL Server
TrackingApp.Backend/UnitTests/    xUnit + EF InMemory (own csproj)
TrackingApp.Frontend/             Vue 3 + Vite + TypeScript (Vue's own src/ stays inside this folder)
```

No repo-root `src/` folder — that name is reserved for Vue sources under `TrackingApp.Frontend/src`.

EF Core + SQL Server live in `TrackingApp.Backend`. Date stored as a calendar `date` (not a UTC instant) because v1 has no time-of-day. Vue keeps a small TypeScript type for the activity JSON; no OpenAPI codegen in v1.

Default local connection: SQL Server LocalDB (`(localdb)\\mssqllocaldb`) and a dedicated database name. Database files may live under `E:\Database`. Override via connection string for full SQL Server, Express, Docker, or a remote instance. Apply migrations on startup in Development (or document `dotnet ef database update`).

**Alternatives considered:** SQLite (zero install; rejected — user wants MSSQL); three C# projects under `src/` (rejected — too much structure for v1); Blazor host (rejected); separate production UI host.

### 5. Single-user, no auth

v1 binds to localhost. Anyone who can reach the process can read/write activities. Record this as accepted for a personal PC tool; add auth when the API is exposed to a phone over the network.

**Alternatives considered:** dummy auth now (cost with no user); API keys (premature until MAUI + non-localhost).

## Risks / Trade-offs

- [SQL Server not installed] → Default to LocalDB in README; connection string is the switch for Express/full/Docker. Fail clearly at startup if the server is unreachable.
- [Two-language stack] → Accept: C# API, TypeScript Vue. Keep types thin and aligned with the JSON contract.
- [CORS / two ports in dev] → Vite proxy to the API origin; production is same-origin.
- [MAUI on another machine cannot use localhost] → Out of scope; hosting and auth become a later change when the phone client exists.
- [Date without time] → Planned; adding time later is a compatible extension, not a status field.

## Migration Plan

Greenfield. No production data. If the first schema is wrong, drop the database and re-run migrations.

## Open Questions

None that block this change. Hosting the API for MAUI (LAN vs cloud) is deferred until the phone client change.
