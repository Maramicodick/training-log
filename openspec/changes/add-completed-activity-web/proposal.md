## Why

There is no way yet to record trainings that already happened. The first useful product is a personal completed-activity log on PC in the browser, so later planning, calendar, and a phone client have a real object to attach to instead of a status flag on a mixed "session."

## What Changes

- Add a web app on PC where the user can log a completed training (date, title, optional notes) and see a list of past activities.
- Persist activities on the server in a .NET backend with an HTTP API so a future MAUI phone client can use the same contract.
- Treat **completed activity** as its own type. No planned/completed status, no expected-vs-actual fields, no calendar.
- Greenfield: introduce the ASP.NET Core API, SQL Server storage, and a Vue web UI.

## Capabilities

### New Capabilities

- `completed-activities`: Create, list, and persist completed trainings (date, title, optional notes) through the web app and HTTP API.

### Modified Capabilities

- (none)

## Impact

- New solution: one ASP.NET Core project (`TrackingApp.Backend`) for the HTTP JSON API and SQL Server; Vue 3 UI later in `frontend/`. The Vue app does not share UI with MAUI.
- No auth, no multi-user accounts, no MAUI project, no planned-workout type in this change.
- Phone remains out of scope; the web app may still be opened in a mobile browser, but that is not a supported target yet.
