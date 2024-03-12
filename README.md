# Networking Course - Jens Sundqvist
## Overall Score 9 points, going for G

### Task 1: Overhead Names 1pt:
***Tag: 1.OverheadNames***

Done by getting the client data and then saving it as a 32bit-string which is parsed into a string and applied to a text field in the player prefab.

*Created Classes: NameUI*

*Affected Prefab: Player*

### Task 2: Health Packs 1pt
***Tag: 2.HealthPacks***

Done by adding a new function to the Health class, Healing. Applies healing the same way as the mine deals damage, not respawnable.
Later tweaked in Tag 6.AmmoPickup

*Affected Classes: Health*

*Created Classes: HealingKit*

*Created Prefabs: Healing Kit* 

### Task 3: Sprite Renderer 1pt
***Tag: 3. Sprite Renderer***

Done by creating a network variable for speed which is updated by the owner, each client then updates their ship's color based on the velocity set.

*Created Classes: SpriteHandler*

*Affected Prefabs: Player*

### Task 4: Limited Ammo 1pt
***Tag: 4.LimitedAmmo***

Done by creating a network variable the server has authority over and then checking if there's ammo available when the player attempts to shoot.
The player knows their ammo count by an ammo counter on the player.

*Affected Classes: FiringAction*

*Affected Prefabs: Player*

### Task 6: Ammo Pickup 1pt
***Tag: 6.AmmoPickup***

Done by creating a new refill ammo function in FiringAction that allows the server to give ammo to the player.
The function is called by a new type of "mine"-prefab, here I put in a Network Transform instead of the old system as that would cause issues if a player joined mid-game.

*Affected Classes: Firing Action*

*Created Classes: AmmoPickup*

*Created Prefabs: AmmoKit*


### Task 9 Player Death 1pt:
***Tag 9.PlayerDeath***

This one went through a few passes before I was happy with it, the server has authority over a *dead* variable. When the server applies damage to the player it updates the *dead* variable.
If the server reports that the player is dead, the client is teleported away and a deathscreen is shown.

*Affected Classes: Health*

*Affected Prefabs(Earlier commits for the same task): Player*

### Task 10 & 11, Player Respawn + Limited Respawn 3pts
***Tag 10.11.(Un)Limited Respawn***

Done by having the client send a respawn request to the server if they're dead. If the player has respawn tokens, which the server has authority over, the player will spawn with their health and ammo restocked.
To make it unlimited, simply remove the respawn token check serverside.

*Affected Classes: Health*
