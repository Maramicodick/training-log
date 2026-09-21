## 1. Activity fields and validation

- [ ] 1.1 Add optional sport, duration minutes, and distance kilometers to the activity and to the create and update requests, and verify `dotnet build` of `TrackingApp.Backend` succeeds.
- [ ] 1.2 Validate sport against Run, RoadRide, Swim, Strength, and Other, require a positive whole number of minutes and a positive kilometer distance when those values are present, and allow them to be omitted. Verify unit tests: a valid Run/45/8 create is stored; omitted fields stay empty; an unknown sport, zero duration, and zero distance each throw and do not persist.

## 2. Persistence

- [ ] 2.1 Add an EF migration for the three nullable columns and verify `dotnet ef database update` (or startup migrate) applies it and existing activities still load.
- [ ] 2.2 Verify a unit test that creates two activities, one with sport, duration, and distance and one without, and lists them with those values intact and the existing date order unchanged.

## 3. API

- [ ] 3.1 Accept and return the new fields on `POST /api/activities` and `GET /api/activities`, and verify a valid body is listed with those values and a whitespace title still returns 400 text and does not persist.
- [ ] 3.2 Accept the new fields on `PATCH /api/activities/{id}`, including clearing them with null, and verify an update changes the listed values, a bad sport returns 400 text and leaves the row unchanged, and an unknown id returns 404 text.

## 4. Vue form and list

- [ ] 4.1 Add optional sport, duration, and distance to the create form and show them on the list when set. Verify a created activity appears with those values, and leaving them empty still creates the activity.
- [ ] 4.2 Let the user edit a saved activity in the same form and save with PATCH, including clearing the optional fields. Verify the list shows the edited values, a whitespace title is rejected and the previous title remains, and a restart still shows the edited sport, duration, and distance.
