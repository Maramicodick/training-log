## Purpose

Lets the user schedule a session they intend to do, separately from the log of sessions they already completed, and turn that plan into a linked completed activity when they do it.

## ADDED Requirements

### Requirement: User can schedule a planned session

The system SHALL allow the user to record a planned session with a date, a sport, a title, and optional duration and distance. Sport MUST be one of Run, RoadRide, Swim, Strength, or Other. RoadRide means road cycling. Title MUST be non-empty after trimming whitespace. Date MUST be present. Duration, when present, MUST be a whole number of minutes greater than zero. Distance, when present, MUST be a number of kilometers greater than zero. A planned session SHALL NOT be stored as a completed activity.

#### Scenario: Schedule with duration and distance

- **GIVEN** the user is adding a planned session
- **WHEN** they submit a date, sport RoadRide, a non-empty title, duration 90 minutes, and distance 40 kilometers
- **THEN** the session is stored on the planned page with those values and does not appear in the completed activity list

#### Scenario: Schedule without duration or distance

- **GIVEN** the user is adding a planned session
- **WHEN** they submit a date, a sport, and a non-empty title and leave duration and distance empty
- **THEN** the session is stored without duration or distance

#### Scenario: Reject an invalid planned session

- **GIVEN** the user is adding a planned session
- **WHEN** they omit the sport, submit a sport outside the allowed set, a whitespace title, a non-positive duration, or a non-positive distance
- **THEN** the session is not stored and the user is shown what was rejected

### Requirement: User can view planned sessions

The system SHALL show planned sessions on their own page, separate from completed activities, ordered by date ascending (soonest first). Sessions on the same date SHALL appear with the most recently added last. Each session SHALL show its date, sport, and title, and duration and distance when those values exist. A session that has been marked done SHALL show that it is done.

#### Scenario: Planned page is separate

- **GIVEN** a planned session and a completed activity both exist
- **WHEN** the user opens the planned page
- **THEN** the planned session is listed and the completed activity is not

#### Scenario: Soonest date first

- **GIVEN** planned sessions exist on different dates
- **WHEN** the user opens the planned page
- **THEN** the session with the earliest date appears first

### Requirement: User can edit or delete a planned session

The system SHALL allow the user to change the date, sport, title, duration, and distance of a planned session, and to delete it. Title, date, and sport rules MUST still apply. The user MUST be able to clear duration and distance. Editing or deleting a planned session SHALL NOT change or delete a linked completed activity. Deleting a planned session SHALL leave the linked completed activity in the completed list without that link.

#### Scenario: Edit a plan

- **GIVEN** a saved planned session
- **WHEN** the user changes its title and saves
- **THEN** the planned page shows the new title

#### Scenario: Delete a plan that was marked done

- **GIVEN** a planned session linked to a completed activity
- **WHEN** the user deletes the planned session
- **THEN** the planned session is gone and the completed activity remains without that link

### Requirement: User can mark a planned session done

The system SHALL let the user mark a planned session done. That action SHALL create one completed activity with the plan's date, sport, title, duration, and distance, and SHALL link that activity to the plan. The plan SHALL remain a planned session and SHALL show as done. Marking a session done a second time SHALL be rejected and SHALL NOT create another activity.

#### Scenario: Mark done

- **GIVEN** a planned session that is not done
- **WHEN** the user marks it done
- **THEN** a completed activity exists with the same date, sport, title, duration, and distance, the two are linked, and the planned page shows the session as done

#### Scenario: Mark done twice

- **GIVEN** a planned session that is already done
- **WHEN** the user marks it done again
- **THEN** no additional completed activity is created and the user is shown that it is already done

### Requirement: HTTP API for planned sessions

The system SHALL expose an HTTP JSON API that can list, create, update, and delete planned sessions, and mark one done. Create, update, and mark-done MUST apply the same validation as the planned form. Mark-done MUST return the created completed activity id. A second mark-done MUST respond with 400 and a text message. Validation failures MUST respond with 400 and a text message.

#### Scenario: Create and list via API

- **GIVEN** a valid JSON body with date, sport, and a non-empty title
- **WHEN** a client creates a planned session and then requests the planned list
- **THEN** the list includes that session and does not include it in the completed activity list

#### Scenario: Mark done via API

- **GIVEN** a stored planned session that is not done
- **WHEN** a client marks it done through the API
- **THEN** the response includes the new completed activity id and a later planned list shows that session as done
