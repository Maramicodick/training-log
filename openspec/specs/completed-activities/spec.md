# completed-activities Specification

## Purpose
Lets the user record trainings that already happened and review them later, as completed activities only.

## Requirements

### Requirement: User can log a completed activity

The system SHALL allow the user to record a completed activity with a date, a title, and optional notes. The system SHALL NOT represent an activity as planned or completed via a status field. Title MUST be non-empty after trimming whitespace. Date MUST be present.

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

### Requirement: User can view completed activities

The system SHALL show stored completed activities in a list ordered by date descending (newest date first). Activities with the same date SHALL appear with the most recently added first.

#### Scenario: Empty list

- **GIVEN** no completed activities exist
- **WHEN** the user opens the activity list
- **THEN** the system shows that there are no activities yet

#### Scenario: Multiple activities

- **GIVEN** several completed activities exist on different dates
- **WHEN** the user opens the activity list
- **THEN** each activity shows date, title, and notes (if any), ordered by date descending

### Requirement: Completed activities persist

The system SHALL persist completed activities so they remain available after the application is restarted.

#### Scenario: Restart after logging

- **GIVEN** the user has logged at least one completed activity
- **WHEN** the application is restarted
- **THEN** those activities still appear in the list with the same date, title, and notes

### Requirement: HTTP API for completed activities

The system SHALL expose an HTTP JSON API that can list completed activities and create a completed activity with date, title, and optional notes. Create MUST apply the same validation as the web form (non-empty title, date required). The API exists so a future native client can use the same contract.

#### Scenario: Create via API

- **GIVEN** a valid JSON body with date and non-empty title
- **WHEN** a client creates a completed activity through the API
- **THEN** the activity is stored and is returned in subsequent list responses

#### Scenario: List via API

- **GIVEN** stored completed activities
- **WHEN** a client requests the activity list through the API
- **THEN** the response includes those activities ordered by date descending
