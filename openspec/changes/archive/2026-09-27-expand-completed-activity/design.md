## Context

See proposal.md for motivation. Completed activities already persist in SQL Server through `TrackingApp.Backend` and are created and listed in `TrackingApp.Frontend`. Update already exists as `PATCH /api/activities/{id}` for date, title, and notes. Validation failures return 400 with a text message.

## Goals / Non-Goals

**Goals:**

- Store a required sport, plus optional duration and distance, on the existing activity row.
- Accept and return those fields on create, list, and update.
- Let the Vue form set them on create and edit a saved row in place.

**Non-Goals:**

- Pace, heart rate, power, elevation, cadence, training load, or a clock time.
- Planned workouts or a planned-versus-completed pair.
- A new project, auth, or a second database.

## Decisions

### 1. Sport is required; duration and distance are optional columns

Add `Sport` (string, required), `DurationMinutes` (int, nullable), and `DistanceKilometers` (decimal, nullable) to `Activity`. Empty duration and distance stay null. There are no users yet, so existing rows can be given `Other` during the migration.

**Alternatives considered:** A separate details table (extra join for three columns); a nullable sport (only needed to keep old rows unchanged).

### 2. Sport is an ActivityType enum stored as text

`ActivityType` is `Run`, `RoadRide`, `Swim`, `Strength`, and `Other`. `Activity.Sport` is required. Entity Framework stores the name (`RoadRide`), and the JSON field is that same string. `RoadRide` is road cycling only. A later mountain, gravel, or indoor type is a new enum name. A missing sport or anything outside the set is a 400 text error from the service, same as a bad title.

**Alternatives considered:** A generic `Ride` (rejected — cycling will split into several types); an integer column (inserting a value renumbers stored rows); a Sports lookup table (extra structure for a fixed list); free text (harder to filter later).

### 3. Duration is whole minutes; distance is kilometers

`DurationMinutes` must be an integer greater than zero when present. `DistanceKilometers` must be greater than zero when present. The API uses those names. The form can label them "Duration (minutes)" and "Distance (km)".

**Alternatives considered:** Seconds and meters (closer to Garmin fit files, awkward to type); miles (not the unit this log will standardize on); a time-span string (harder to validate).

### 4. Edit uses the existing PATCH

The same request shape as create, including sport, duration, and distance. Omitting or sending null clears duration and distance. Sport stays required. The controller stays thin: `ArgumentException` becomes 400 text, `KeyNotFoundException` becomes 404 text. Unit tests use the in-memory database.

**Alternatives considered:** PUT (replaced earlier by PATCH); a separate edit endpoint; field-error dictionaries (rejected).

### 5. One form for add and edit

The Vue page keeps a single form. Edit loads the selected activity into it and PATCHes on save. The list always shows the sport, and shows duration and distance only when set. Styling stays plain so it can be changed by hand.

**Alternatives considered:** A second edit page (more routing than this slice needs).

## Risks / Trade-offs

- [Whole minutes drop seconds] → Acceptable until a later change needs a clock duration.
- [Kilometers only] → A later display preference can convert; storage stays kilometers.
- [Existing rows have no sport] → The migration sets them to `Other`. The sample log can be edited or deleted afterward.

## Migration Plan

Add an EF migration: required `Sport` stored as text, nullable duration and distance. Existing rows get `Other`. Apply it on startup the same way as the initial migration. Rollback is dropping those three columns.

## Open Questions

None.
