# Changelog

## [0.3.0] – 2025-07-21
### Summary

Implemented session modifiers that introduce additional logic into host-customer sessions.

### Changed
- Reworked the charge customer logic: charging is now handled through a session modifier. The main session code is now focused solely on the timer loop.
- Added basic host traits.
- Added basic customer preferences.
- Added a compatibility calculator between host traits and customer preferences.
- Introduced a compatibility-based session modifier, which applies a bonus or penalty to the amount of money a host receives based on compatibility.
- Added simple customer reaction effects to compatibility (using the particle system).

---

## [0.2.0] – 2025-07-12
### Summary
Implement A* algorhythm for characters' movement.

### Changed
- Waypoints movement is completely reworked into A* pathfinding algorhythm
- Existing code is refactored to use A* characters movements

---

## [0.1.1] – 2025-07-06
### Summary
Fixed a critical bug

### Fixed
- Resolved critical bug which prevented a host to be assigned to a table on arrival

---

## [0.1.0] – 2025-07-06
### Summary
Initial milestone build containing all foundational systems, features, and logic prototypes. Marks the end of pre-dev experimentation and the beginning of structured development.

### Added
- Simple customer invitation system
- Simple host and customer state machines
- Host assignment and reassignment system
- Customer movement through waypoint paths
- Exit zones for character disappearance after sessions
- Reception zone logic for customer intake
- Shift system with pre-shift UI, in-shift UI, and post-shift results panel
- Animated pulsing selection UI for tables
- UI element for customer waiting countdown
- Core scene with tilemap, player movement, and camera bounds

### Changed
- Major refactoring and cleanup of core scripts
- Moved and reorganized systems like `CustomerInvitationManager`
- Separated shift UI layers for better clarity
- Adjusted invitation timing and cleaned redundant code

### Fixed
- Host not returning to pool after session
- Prevented collisions between player, host, and customer
- Fixed bugs with session lifecycle and assignment logic
- Resolved initial spawn and logic ordering issues

### Refs
HCLUB-17, HCLUB-19, HCLUB-26, HCLUB-34, HCLUB-36, HCLUB-37