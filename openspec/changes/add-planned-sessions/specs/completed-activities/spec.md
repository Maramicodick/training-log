## ADDED Requirements

### Requirement: Completed activity can link to a planned session

The system SHALL allow a completed activity to record at most one planned session that it fulfilled. An activity with no plan SHALL have no such link. Creating and editing a completed activity without a plan SHALL stay as they are today. The completed list SHALL show when an activity is linked to a planned session.

#### Scenario: Activity with no plan

- **GIVEN** the user logs a completed activity without marking a plan done
- **WHEN** they view the completed list
- **THEN** that activity has no planned-session link

#### Scenario: Activity created from a plan

- **GIVEN** the user marks a planned session done
- **WHEN** they view the completed list
- **THEN** the new activity shows that it is linked to that planned session

### Requirement: Completed activities have their own page

The system SHALL show completed activities on a page of their own, separate from planned sessions. The user SHALL be able to move from that page to the planned page and back. Opening the app SHALL show the completed page.

#### Scenario: Completed page is separate

- **GIVEN** a completed activity and a planned session both exist
- **WHEN** the user opens the completed page
- **THEN** the activity is listed and the planned session is not

#### Scenario: Move to the planned page

- **GIVEN** the user is on the completed page
- **WHEN** they open the planned page and then return
- **THEN** each page shows only its own records
