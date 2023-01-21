# Details

This is a core item randomizer. It places chests around the map in set locations and uses [Logic](https://github.com/jet082/Core-Item-and-Info-Randomizer/blob/main/Core%20Item%20and%20Info%20Randomizer/DefaultLogic.json) to determine placements.

# Credits

The full credits for the project can be found [here](https://github.com/jet082/Core-Item-and-Info-Randomizer/blob/main/CREDITS.md).

# TO DO

# Tech

* Maybe we can remove the Mobile Vehicle Bay from the pool or make it a junk item. Maybe make it an option to require the MVB to build vehicles from 'vehicle kits'
* Convert Choose Your Spawns to code so it can be toggled on/off.
* Figure out how to make sure the randomizer can be toggled on/off so it doesn't impact other mods too heavily.
* Figure out how to add options, etc.
* A nice-to-have would be to do an update to BaseKits to use a mini version of the buildings in question rather than the generic loot cube.

## Logic

* Keep working on logic. Write logic parser.

## Placement

* Add tons more chests. Maybe add a random-ish amount per region? Like 5+/-2 or something to not necessarily make each location of consistent value and add more of a gamble? Unsure.

# Ideas for the Future

## Door Access Codes
* **SHORT TERM**. There should be one 50/50 chance lie for each door access code. Guaranteed access code description is "The code to the <location> is 0000" vs "The code to the <location> might be 0000" with a 50/50 chance of being accurate. This adds some gambling to the rando, which is good.
* **SHORT TERM**. Since we maybe won't need the base blueprints, maybe make the Captain's Cabin be a treasure trove of checks idk.

## Misc
* **LONG TERM**. Support for transition randomizer
* **MID TERM**. Make 'finding the world's only fabricator to make the cure' a thing. Place a fabricator in one of like 5? different locations.

## PDA
* **SHORT TERM**. List of all possible check locations.
	* **MID TERM**. Maybe build logic in to show what's currently accessible.
* **LONG TERM**. Maybe add in an 'all possible Fishy Fact/Lie' list that adds entries (across game saves) when new Fishy Facts/Lies are seen. That way junk hints still have some excitement to them.
	* **VERY LONG TERM**. Maybe add unlockables to completing the master fact/lie database? Some could be bad too - "UNLOCKED A NEW REAPER LEVIATHAN IN THE SAFE SHALLOWS" lol.
