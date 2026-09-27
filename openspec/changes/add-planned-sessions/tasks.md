## 1. Planned session model

- [ ] 1.1 Add a planned-session entity with date, sport, title, optional duration, and optional distance, plus an optional unique link from a completed activity to one planned session, and verify `dotnet build` of `TrackingApp.Backend` succeeds.
- [ ] 1.2 Validate planned sessions with the same sport, title, duration, and distance rules as activities, and verify unit tests: a valid RoadRide plan is stored and is not an activity; a missing sport or whitespace title throws and does not persist.

## 2. Persistence

- [ ] 2.1 Add an EF migration for the planned-session table and the nullable unique activity link, and verify startup migrate applies it and existing activities still load with no link.
- [ ] 2.2 Verify a unit test that stores two planned sessions on different dates and lists them soonest first, with duration and distance intact on the one that has them.

## 3. Mark done and API

- [ ] 3.1 Mark a planned session done by creating a linked completed activity with the plan's date, sport, title, duration, and distance, and verify a unit test: the activity is linked, a second mark-done throws, and deleting the plan leaves the activity with no link.
- [ ] 3.2 Expose list, create, update, delete, and mark-done on `/api/planned-sessions`, and verify a valid create is absent from `GET /api/activities`, mark-done returns the new activity id, a second mark-done returns 400 text, and a bad title returns 400 text and does not persist.

## 4. Vue pages

- [ ] 4.1 Add a completed page at `/completed` and a planned page at `/planned`, with navigation between them. `/` opens the completed page. Verify each page lists only its own records, planned sessions are soonest date first, and a missing sport does not create a plan.
- [ ] 4.2 Add mark done on the planned page. Verify the completed page shows the new linked activity, the plan shows as done, marking it done again does not add another activity, and a restart still shows the link.
