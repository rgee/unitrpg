# Battle Model

- chapterIndex
- units: Unit[]
  - id
  - position
  - characterName
  - health
  - items: Item[]
- currentTurn
  - phase: Friendly | Enemy
  - count
  - actions: Action[]
    - unitId
    - type: Move | Action
    - distanceMoved (optional)

## Notes
- When the battle is initialized, if the current save has one of these, the
battle will be initialized to this state. If not, it'll use a pre-configured
default.

- If initializing battle from a save state, intro dialogue / scripting is
skipped.
