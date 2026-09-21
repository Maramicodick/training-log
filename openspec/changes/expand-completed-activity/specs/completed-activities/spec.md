## MODIFIED Requirements

### Requirement: User can log a completed activity

The system SHALL allow the user to record a completed activity with a date, a title, and optional notes, sport, duration, and distance. The system SHALL NOT represent an activity as planned or completed via a status field. Title MUST be non-empty after trimming whitespace. Date MUST be present. Sport, when present, MUST be one of Run, RoadRide, Swim, Strength, or Other. RoadRide means road cycling. Duration, when present, MUST be a whole number of minutes greater than zero. Distance, when present, MUST be a number of kilometers greater than zero.

#### Scenario: Log activity with all fields

- **GIVEN** the user is adding a completed activity
- **WHEN** they submit a date, a non-empty title, and notes
- **THEN** the activity is stored and appears in the activity list with those values

#### Scenario: Log activity without notes

- **GIVEN** the user is adding a completed activity
- **WHEN** they submit a date and a non-empty title and leave notes empty
- **THEN** the activity is stored and appears in the list with empty notes

#### Scenario: Reject missing title

- **GIVEN** the user is adding a completed activity
- **WHEN** they submit without a title, or with only whitespace
- **THEN** the activity is not stored and the user is shown that a title is required

#### Scenario: Default date is today

- **GIVEN** the user opens the form to add a completed activity
- **WHEN** they have not changed the date
- **THEN** the date is today's local date

#### Scenario: Log sport, duration, and distance

- **GIVEN** the user is adding a completed activity
- **WHEN** they submit a date, a non-empty title, sport Run, duration 45 minutes, and distance 8 kilometers
- **THEN** the activity is stored and appears in the list with those values

#### Scenario: Log without sport, duration, or distance

- **GIVEN** the user is adding a completed activity
- **WHEN** they submit a date and a non-empty title and leave sport, duration, and distance empty
- **THEN** the activity is stored and appears in the list without those values

#### Scenario: Reject invalid sport, duration, or distance

- **GIVEN** the user is adding a completed activity
- **WHEN** they submit a sport outside the allowed set, a duration that is not a positive whole number of minutes, or a distance that is not a positive number of kilometers
- **THEN** the activity is not stored and the user is shown what was rejected

### Requirement: User can view completed activities

The system SHALL show stored completed activities in a list ordered by date descending (newest date first). Activities with the same date SHALL appear with the most recently added first. Each activity SHALL show its date, title, and notes when notes exist, and SHALL show sport, duration, and distance when those values exist.

#### Scenario: Empty list

- **GIVEN** no completed activities exist
- **WHEN** the user opens the activity list
- **THEN** the system shows that there are no activities yet

#### Scenario: Multiple activities

- **GIVEN** several completed activities exist on different dates
- **WHEN** the user opens the activity list
- **THEN** each activity shows date, title, and notes (if any), ordered by date descending

#### Scenario: Activity with sport, duration, and distance

- **GIVEN** a completed activity has sport, duration, and distance
- **WHEN** the user opens the activity list
- **THEN** that activity shows those values along with its date and title

### Requirement: Completed activities persist

The system SHALL persist completed activities so they remain available after the application is restarted, including sport, duration, and distance when they were set.

#### Scenario: Restart after logging

- **GIVEN** the user has logged at least one completed activity
- **WHEN** the application is restarted
- **THEN** those activities still appear in the list with the same date, title, and notes

#### Scenario: Restart keeps sport, duration, and distance

- **GIVEN** the user has logged a completed activity with sport, duration, and distance
- **WHEN** the application is restarted
- **THEN** that activity still shows the same sport, duration, and distance

### Requirement: HTTP API for completed activities

The system SHALL expose an HTTP JSON API that can list completed activities, create a completed activity, and update a completed activity. Create and update accept date, title, optional notes, optional sport, optional duration, and optional distance. Create and update MUST apply the same validation (non-empty title, date required, and the sport, duration, and distance rules). The API exists so a future native client can use the same contract.

#### Scenario: Create via API

- **GIVEN** a valid JSON body with date and non-empty title
- **WHEN** a client creates a completed activity through the API
- **THEN** the activity is stored and is returned in subsequent list responses

#### Scenario: List via API

- **GIVEN** stored completed activities
- **WHEN** a client requests the activity list through the API
- **THEN** the response includes those activities ordered by date descending

#### Scenario: Create with sport, duration, and distance via API

- **GIVEN** a valid JSON body with date, non-empty title, sport, duration, and distance
- **WHEN** a client creates a completed activity through the API
- **THEN** a subsequent list response includes those values for that activity

#### Scenario: Reject invalid body via API

- **GIVEN** a JSON body with a whitespace title, a sport outside the allowed set, a non-positive duration, or a non-positive distance
- **WHEN** a client creates or updates a completed activity through the API
- **THEN** the API responds with 400 and a text message, and the stored activities are unchanged

## ADDED Requirements

### Requirement: User can edit a saved completed activity

The system SHALL allow the user to change the date, title, notes, sport, duration, and distance of a saved completed activity. Title and date rules MUST still apply. The user MUST be able to clear sport, duration, and distance. After a successful edit, the list SHALL show the updated values.

#### Scenario: Edit saved fields

- **GIVEN** a saved completed activity
- **WHEN** the user changes its title, date, notes, sport, duration, and distance to valid values and saves
- **THEN** the list shows the new values

#### Scenario: Clear optional fields

- **GIVEN** a saved completed activity that has sport, duration, and distance
- **WHEN** the user clears those three fields and saves
- **THEN** the list shows the activity without sport, duration, or distance

#### Scenario: Reject invalid edit

- **GIVEN** a saved completed activity
- **WHEN** the user saves a whitespace title, a sport outside the allowed set, a non-positive duration, or a non-positive distance
- **THEN** the activity is unchanged and the user is shown what was rejected
