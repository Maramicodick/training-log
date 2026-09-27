## Why

Completed activities are a record of what already happened. Scheduling the next session needs its own record and page, so a plan does not become a status on an activity. The two stay easy to tell apart, and a plan can still be linked when the user does it.

## What Changes

- Add a planned session with a date, a required sport, a title, and optional duration and distance. Sport uses the same set as completed activities: Run, RoadRide, Swim, Strength, Other. RoadRide means road cycling.
- Give completed activities their own page at `/completed`, and planned sessions their own page at `/planned`. The app opens on the completed page. The user can move between the two pages.
- Show planned sessions ordered by date ascending (soonest first). Completed activities stay ordered by date descending.
- Let the user edit and delete a planned session. Editing a plan does not change a linked completed activity.
- Add "Mark done": it creates a completed activity from the plan (same date, sport, title, duration, and distance) and links them. The plan remains a plan and shows that it was done.
- A completed activity may link to at most one planned session. A planned session may link to at most one completed activity. Activities with no plan stay unlinked.
- Assumption: no calendar grid, no automatic matching, and no file import in this change.

## Capabilities

### New Capabilities

- `planned-sessions`: Schedule a session, review it on its own page, and mark it done by creating a linked completed activity.

### Modified Capabilities

- `completed-activities`: A completed activity may record which planned session it fulfilled. Creating and editing an activity otherwise stay the same.

## Impact

- New planned-session storage, API, and Vue page. The current single screen becomes the completed page. Completed activities gain an optional link.
- Navigation between `/completed` and `/planned`.
- No new app, no auth, and no change to how an unlinked activity is logged.
