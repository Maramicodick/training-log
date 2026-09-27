## Context

See proposal.md for motivation. Completed activities already live in `Activity` with a required `ActivityType` stored as text. The Vue app is one page in `App.vue`.

## Goals / Non-Goals

**Goals:**

- Store a planned session in its own table and show it on its own page.
- Link at most one completed activity to a planned session.
- Mark done creates that activity from the plan and sets the link.

**Non-Goals:**

- A calendar grid, automatic matching, or file import.
- Copying later edits from the plan onto the activity, or from the activity back onto the plan.
- Changing how an unlinked activity is created.

## Decisions

### 1. A planned session is its own row

`PlannedSession` has id, date, sport, title, optional duration minutes, and optional distance kilometers. Sport reuses `ActivityType` and is stored as text. Validation matches completed activities. It is not a second kind of `Activity`.

**Alternatives considered:** Planned and actual columns on `Activity` (rejected — the user wants two records); a status flag on `Activity` (rejected earlier).

### 2. The link sits on the completed activity

`Activity.PlannedSessionId` is an optional foreign key. A unique index makes it one activity per plan. Mark done inserts the activity with that id set. Deleting a plan sets the activity's link to null and leaves the activity. Deleting an activity leaves the plan, which is then not done.

**Alternatives considered:** A foreign key on the plan pointing at the activity (the plan would change shape when completed); a join table (extra table for a one-to-one link).

### 3. Mark done is its own action

`POST /api/planned-sessions/{id}/done` creates the activity and returns its id. A plan that already has a link returns 400 with a text message. The other planned routes are list, create, update, and delete, with the same 400 text style as activities.

**Alternatives considered:** Updating the plan with a completed flag and writing the activity in the client (the link could be skipped); turning the plan row into an activity (loses the plan).

### 4. Two Vue pages

Add Vue Router. Completed activities move to `/completed`. Planned sessions are at `/planned`. `/` opens the completed page. Each page has its own form and list, and a navigation link to the other page. Mark done stays on the planned page. Styling stays plain.

**Alternatives considered:** Leaving completed activities at `/` with no completed path (the user wants a completed page too); one page with a toggle and no router; a calendar view (out of scope).

## Risks / Trade-offs

- [Mark done copies values once] → Later edits to either side do not sync. The link still shows they belong together.
- [Deleting a plan clears the link] → The completed activity remains, so the log is not lost.

## Migration Plan

Add the `PlannedSessions` table and a nullable unique `PlannedSessionId` on `Activities`. Existing activities stay unlinked. Rollback drops the column and the table.

## Open Questions

None.
