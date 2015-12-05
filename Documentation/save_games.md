# Save Games
- characters
- lastChapterCompleted: `int`
- currentBattle: `Optional<Battle>`

## Notes
- Binary serialization
- Singleton SaveGameService
- Service provides current save game, listing available saves, loading
a specific save, and overwriting saves.
