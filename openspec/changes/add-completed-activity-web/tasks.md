## 1. Solution skeleton

- [ ] 1.1 Create `src/TrackingApp.Contracts`, `src/TrackingApp.App`, and `src/TrackingApp.Web` (ASP.NET Core web API, no Blazor) in a solution at the repo root, and verify `dotnet build` succeeds.
- [ ] 1.2 Add EF Core SQL Server to `TrackingApp.App`, read a connection string from configuration (default LocalDB), and verify the web project references App and Contracts and still builds.
- [ ] 1.3 Scaffold `src/TrackingApp.Web.Client` as Vue 3 + Vite + TypeScript, and verify `npm install` and `npm run build` succeed.

## 2. Domain, persistence, and API

- [ ] 2.1 Add completed-activity DTOs and shared title/date validation in Contracts (no status field) and verify invalid titles (empty/whitespace) fail validation in a unit test.
- [ ] 2.2 Persist activities with GUID id, calendar date, title, notes, and created-at; list by date descending then most recently added; verify an App-layer test: create two rows and list order matches.
- [ ] 2.3 Expose `GET /api/activities` and `POST /api/activities` using that validation, returning 400 with field errors on bad title/date, and verify with WebApplicationFactory or equivalent: POST valid body then GET includes it; POST whitespace title returns 400 and does not persist.
- [ ] 2.4 Configure Vite to proxy `/api` to the ASP.NET host in development, and verify a browser request to the Vue origin for `GET /api/activities` reaches the API.

## 3. Vue UI

- [ ] 3.1 Add an activity list view that shows empty state when none exist and otherwise date, title, and notes, ordered as specified, loaded via `GET /api/activities`; verify empty state and a list after API-created rows.
- [ ] 3.2 Add a create form with date defaulting to today's local date, required title, optional notes, posting to `POST /api/activities`, and show API/field errors for missing/whitespace title; verify default date, successful create appears on the list, and whitespace title is rejected without a new row.
- [ ] 3.3 Configure the API host to serve the Vue production build from `wwwroot` (or equivalent), and verify `dotnet run` on the web project serves both the UI and the API on one origin.

## 4. Persistence check and docs

- [ ] 4.1 Run the app, create an activity in the Vue UI, stop and start the process, and verify the activity is still listed (same date, title, notes).
- [ ] 4.2 Add a short README with how to run API + Vue in dev, how to run the combined host, that the API is unauthenticated on localhost, LocalDB/SQL Server prerequisites, and how to set the connection string; verify those steps match the implementation.
