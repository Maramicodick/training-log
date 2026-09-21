## Why

A completed activity is only a date, title, and notes. TrainingPeaks and Garmin always record the sport plus how long and how far the session was, and both let you correct a saved activity. This change adds that small set so the log is useful before heart rate, power, or planned workouts.

## What Changes

- Add optional sport, duration, and distance on a completed activity. Existing activities stay valid without them.
- Sport is one of Run, RoadRide, Swim, Strength, or Other. RoadRide means road cycling. Other cycling types can be added later as their own values.
- Duration is whole minutes and must be greater than zero when present.
- Distance is kilometers and must be greater than zero when present.
- Show those values on the activity list when they are set.
- Let the user edit a saved activity (date, title, notes, sport, duration, distance). Title and date rules stay the same. Optional fields can be cleared.
- Assumption: no pace, heart rate, power, elevation, cadence, training load, start time, or planned-versus-completed fields in this change.

## Capabilities

### New Capabilities

- (none)

### Modified Capabilities

- `completed-activities`: Logging, listing, persistence, and the HTTP API gain optional sport, duration, and distance, and a saved activity can be updated.

## Impact

- Completed-activity create, list, and update behavior in the API and the Vue form.
- Stored activities gain the new optional fields. Activities saved before this change remain listable and editable.
- No new app, no auth, no planned-workout type.
