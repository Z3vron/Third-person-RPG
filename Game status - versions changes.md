This is my game - there are lots of bugs and issues with it but i am slowly repeairing them and adding some new features. Game doesn't look to good visualy but i never 
intended to make visually pleasing game - I am not an artist but i like coding so i focus on writing mechanics for the game.

Version 1.04
Fetaures included/done in most part:
- player can walk,jum,sprint and crouch
- rotating camera
- player can attack enemy
- enemy will detect player, chase him and attack him
- player inventory system with diffrent sections(weapons/items/materials)
- crafting inventory
- transfering items between inventories by pressing button ( auto stack items in slots) or by dragging and dropping
- drop items from inv into game world
- splitting itmes from one stack into few stacks
- repairing weapons durability (partly works)
- showing info about item
- object inventory
- bonuses to combat base on weapon class (dagger -> faster attacks) - need more time and ideas but first i want to improve overall combat system
- Scriptable-objects based items
- lock on closest enemy and target switching

Known Bugs:
- Player can jump on and stay on enemy
- Camera shakes while going up the stairs
- enemy sometimes when following player has big turn radious 
- if in the arrea there are many dropped items and there is also an object to interact than errores sometimes occures - bad way of handling that in code - find new better way
- if player hit ceiling he could stay there for a momenmt - character controleer is trying to move it but it cann't - my attempts of pushing player down didn't success
- 

Things that definitely need redoing:
- animator system - for now i have almost none animations so that isn't such a big issue but later on, it will become massive one if i stay with current approach
- equip weapon into hand - i have 2 pivots (one for left and second for right habd) on each weapon and scripts don't work with each other ( look into  order of execution of scripts)
- player current speed - i need it to change animations transitions - right now it is turned off because values are not stable and have big spread - there is a way with using only one character controller but i run into some issues with that
- enemy choosing which attack to use - it's not random - its done strange (badly - i was tired)
- movement system while in lock on enemy mode - going towards and form the enemy is ok but right ot left has got bad feeling to it( think on camera settings)
- dashes

Things i want to do/add:
- blocking enemy attacks
- cast some spells(maybe) - mainly some particle effects
- create some test for the nav mesh agent - to try if he can walk on it without an issue and also atack from them - check if attack moves enemy into wall etc
- enemy patrol state etc
- save system(ScriptableObject?)
- need more animations
- play with the terrain tool
- pro-builder or something like that sounds not bad
-overall optimazation
