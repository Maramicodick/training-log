## Context

See proposal.md for motivation. Completed activities already persist in SQL Server through `TrackingApp.Backend` and are created and listed in `TrackingApp.Frontend`. Update already exists as `PATCH /api/activities/{id}` for date, title, and notes. Validation failures return 400 with a text message.

## Goals / Non-Goals

**Goals:**

- Store optional sport, duration, and distance on the existing activity row.
- Accept and return those fields on create, list, and update.
- Let the Vue form set them on create and edit a saved row in place.

**Non-Goals:**

- Pace, heart rate, power, elevation, cadence, training load, or a clock time.
- Planned workouts or a planned-versus-completed pair.
- A new project, auth, or a second database.

## Decisions

### 1. Optional columns on the existing activity

Add nullable `Sport` (string), `DurationMinutes` (int), and `DistanceKilometers` (decimal) to `Activity`. Empty form values stay null. Existing rows stay valid with nulls.

**Alternatives considered:** A separate details table (extra join for three columns); required sport (would invalidate the recovery-run style entries already saved).

### 2. Sport is an ActivityType enum stored as text

`ActivityType` is `Run`, `RoadRide`, `Swim`, `Strength`, and `Other`. `Activity.Sport` is nullable. Entity Framework stores the name (`RoadRide`), and the JSON field is that same string. `RoadRide` is road cycling only. A later mountain, gravel, or indoor type is a new enum name. Anything outside the set is a 400 text error from the service, same as a bad title.

**Alternatives considered:** A generic `Ride` (rejected — cycling will split into several types); an integer column (inserting a value renumbers stored rows); a Sports lookup table (extra structure for a fixed list); free text (harder to filter later).

### 3. Duration is whole minutes; distance is kilometers

`DurationMinutes` must be an integer greater than zero when present. `DistanceKilometers` must be greater than zero when present. The API uses those names. The form can label them "Duration (minutes)" and "Distance (km)".

**Alternatives considered:** Seconds and meters (closer to Garmin fit files, awkward to type); miles (not the unit this log will standardize on); a time-span string (harder to validate).

### 4. Edit uses the existing PATCH

The same request shape as create, including the new optional fields. Omitting or sending null clears sport, duration, and distance. The controller stays thin: `ArgumentException` becomes 400 text, `KeyNotFoundException` becomes 404 text. Unit tests use the in-memory database.

**Alternatives considered:** PUT (replaced earlier by PATCH); a separate edit endpoint; field-error dictionaries (rejected).

### 5. One form for add and edit

The Vue page keeps a single form. Edit loads the selected activity into it and PATCHes on save. The list shows sport, duration, and distance only when set. Styling stays plain so it can be changed by hand.

**Alternatives considered:** A second edit page (more routing than this slice needs).

## Risks / Trade-offs

- [Whole minutes drop seconds] → Acceptable until a later change needs a clock duration.
- [Kilometers only] → A later display preference can convert; storage stays kilometers.
- [Old rows have null sport, duration, and distance] → List and edit treat null as empty. No backfill.

## Migration Plan

Add a nullable-column EF migration and apply it on startup the same way as the initial migration. Rollback is dropping those three columns; no other table changes.

## Open Questions

None.
