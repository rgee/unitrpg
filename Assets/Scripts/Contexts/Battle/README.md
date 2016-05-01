Combat Scene Context
---

Models
===
* Battle - Holds state about what part of the UI flow the player is in on their turn (attacker selected, etc etc). The state machine implicit here could direct which sub views are active. Those views should have their own model.
* Map - Holds the actual state of the world, the units, the map, etc


Commands
===
* Select Unit - When the player clicks on a unit, if they are friendly, display the action menu
* Select Action - Propose an action for the selected unit. If it's a fight, for example, calculates the combat 
* Confirm Fight - Execute the forecasted action
* Select Move - Elect to move the selected unit
* Confirm Move - Execute the move to the selected square
* Select Info - Show the selected unit's info


Views
===
* Map - The map background and the units
* Pathfinding - The tile overlays for move ranges / attack ranges
* Action Menu - Has buttons for the options available to the player for the selected unit
* Combat Forecast -Shows the damage / hit chances breakdown of the selected action for the selected unit and a confirm/reject button
* Move Pips - Shows the number of remaining moves while selecting a move location 