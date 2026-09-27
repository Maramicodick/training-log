## 1. Activity fields and validation

- [x] 1.1 Add required sport plus optional duration minutes and distance kilometers to the activity and to the create and update requests, and verify `dotnet build` of `TrackingApp.Backend` succeeds.
- [x] 1.2 Validate sport against Run, RoadRide, Swim, Strength, and Other, require a sport, and require a positive whole number of minutes and a positive kilometer distance when those values are present. Verify unit tests: a valid Run/45/8 create is stored; duration and distance may be omitted; a missing sport, an unknown sport, zero duration, and zero distance each throw and do not persist.

## 2. Persistence

- [x] 2.1 Add an EF migration for required sport text plus nullable duration and distance, set existing rows to `Other`, and verify startup migrate applies it and existing activities still load.
- [x] 2.2 Verify a unit test that creates two activities, one with sport, duration, and distance and one with sport only, and lists them with those values intact and the existing date order unchanged.

## 3. API

- [x] 3.1 Accept and return the new fields on `POST /api/activities` and `GET /api/activities`, and verify a valid body is listed with those values and a whitespace title still returns 400 text and does not persist.
- [x] 3.2 Accept the new fields on `PATCH /api/activities/{id}`, including clearing duration and distance with null, and verify an update changes the listed values, a missing or bad sport returns 400 text and leaves the row unchanged, and an unknown id returns 404 text.

## 4. Vue form and list

- [x] 4.1 Add a required sport and optional duration and distance to the create form, and show sport always and duration and distance when set. Verify a created activity appears with those values, leaving duration and distance empty still creates the activity, and a missing sport does not.
- [x] 4.2 Let the user edit a saved activity in the same form and save with PATCH, including clearing duration and distance. Verify the list shows the edited values, a whitespace title or missing sport is rejected and the previous values remain, and a restart still shows the edited sport, duration, and distance.
