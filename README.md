# SWD2Randomizer
Randomizer for the game SteamWorld Dig 2

## What does the Randomizer do?

- It randomizes all upgrades on upgrade podiums (excluding the run boots) together with the three secret blueprints (Blood Quest, Thrillseeker's Tale and Deathplosions) and the triple grenade launcher.
- It randomizes all caves that have one entry
- It randomizes the type of random resources
- It randomizes the available buying upgrades, and adds a few implemented upgrades that didn't make it into the game
- It ensures that the resulting layout is possible to finish.
- It also makes some additional patches (see below)

There are two possible difficulties: `Casual` and `Speedrunner`. In `Speedrunner` difficulty, you may need to:

- Use double jumps and hook jumps
- Use grenade jumps
- Dig with Deathplosions

## Additional patches

To ease the experience, a few other patches are applied to the game:

- The intro cutscene is skipped
- The first boss is always available to fight, even after doing the tutorial sequence break
- The map is available from the start
- The characters in El Machino are not stuck to the initial cutscene when doing the tutorial sequence-break
- The arrows arena from the ToG lower left cave is completed already
- The ignition axe damage against the boss guarding the ignition axe upgrade podium is fixed (it does 5% of base damage in the original game)
- The gate to the hook cave in Oasis is always open
- The triple grenade launcher price is reduced to 200
- The final boss fight is skipped entirely
- The bug that allows players to enter the final boss fight with only the three last generators is fixed. Now all four generators are required. However, the teleporter to the Oasis is never blocked
- The Vector sequence is skipped, players can access the upgrade (jetpack in vanilla game) by destoying the snake rocks with a water bomb.

## Additional notes

- It is possible in a cave that you gain an upgrade but cannot escape the cave. Self-destroy sends you to the upgrade podium, but returning to the main menu and loading your save again sends you to the entrance of the cave
- It is never required to buy the pressure bomb upgrade that lets you break hard rocks

## Using the Randomizer

- Go to the [releases](https://github.com/clementgallet/SWD2Randomizer/releases) section in this repository.
- Download the latest version
- Run the program with the game shutdown
- Fill the path to the base game directory
- In the settings, choose a difficulty and settings, or input a custom seed
- Press `Randomize`. Now you can start the game, and close the randomizer if you want
- You can press `Restore` to recover the original game files. It is needed to use the randomizer again
