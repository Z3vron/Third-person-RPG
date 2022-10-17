I started learning Unity Game Engine by creating my first game which of course isn't perfect (not even acceptably good but i have lots of fun in process of making it).
This is my game - there are lots of bugs and issues with it but i am slowly repeairing them and adding some new features. Game doesn't look to good visualy but i never 
intended to make visually pleasing game - I am not an artist but i like coding so i focus on writing mechanics for the game.
I added some models and animations from the store (ofcourse free assets because i am student) so that game will look more visually pleasing

Think about
- Use profiler for both performance and garbage
- Think on Gc - could use struct rather than classes because struct doesn't create as mach garbage especially when i need container for simple type of data,for simple data eg. no arrayes,lists etc structs should be better, need to check profiler for the Garbage collector allocation, need to check If scriptable objects create garbage - where they are alocated
- Addidng strings create small amount on garbage - i am not using it every frame but might be an option to change in the future
- Debug.log creates lots of garbage ok while working on the game but before building game delete/comment all debug.log lines
- Think on reusing objects
- Use get-set (for example get returns value and sets change it if condition was met)
- Move checking input flags from movement script to input handler - earlier there was a problem with it but i dont remember what was it 

Next update:
- player strong attack interrupt enemy attack( added animations for both player and enemy)
- issue with humanoid animations on generic rig - changing type gives errores
- complete adding animations for first enemy( still missing some nice attack animations)
- Berries are now gathered from bushes
- Added quick potions slots(similar to weapon slots) - equiping, removing from slots and using in all methods(shortcut,drag,item menu option)
- Changed playing sound from the player - still needs to learn about sound in unity because i dont know nothing about it

Update 03.10.2022
- Decided to use some free assets from unity store
- change player model
- Added combat/locked on enemy separate movement
- Added few new weapons/ models from free assets
- Added poison potions - can poison any sword and deal extra damage(armour ignored) to enemy over time - enemy poisoned has got small icon with filler around indicator
- Added shortcuts to change between player inv and crafting inv, equip weapons and poison them
- Added class that calls given function after given amount of time, it creates new object for that time(performance - could make list of classes(containers for info) inside gamemanager (all timers would work around one update function - right now each timer has got separate MonoBehaviour script - seperate update function on separate object))
- Added checking angle to interactable objects so that player could interact with them if they are in front of him
- Changed enemy model/enemy animations handler/ and few values
- Added from store enemy animations (in progress)
- Added rapier class - need to work on critical damage chance etc - randomize stuff overall unknown to me right now
- Created override animator layer for the attacks - in the future to do animations only for some parts of body(avatar mask)
- Fixed bug that allowed jump endlessly by keeping jump button pressed
- Fixed bug that drained stamina when pressed dash button but no direction was given
- Pop-up window containing item info will automaticly refresh itself after user press one of shortcuts on the given item
- Think on Gamemaster - what put inside etc
- Could use spherecast to detect interactable objects - right now i use on trigger enter on player - reapaired some bugs with it - could be done better
- Searched unity store and downloaded and imported some simple animations


Update 06.09.2022
- Changed animations transitions from nodes(in future there would be to much arrows and chaos and bools/triggers) to transitions through code(Crossfade)
- Stamina consumption - sometimes it's tricky - always inconsistent - jump action( function is called many times - framerate is unstable? - look in future)
- Added new item type(inventory row): Potions - health potions restoring health
- Make all animations independent from the player model - cleared some trash combine with it -  stiil need to update player model so that it won't have any animations
- Did some code cleaning - extra functions that work on data of diffrent type (Potions,Materials etc) - think more on Generics
- Added player invulnerability during dashes
- Fixed error connected to killing enemy while not in lock on state
- Fixed error with transfering items between inventories - no idea how it occuried - simple fix but inventories code could me more clean and simple
- Crafting system is looking for ingredients in both material and item list, could optimize to use less code(write extra function for it - maybe using generics - i want to use it)
- Added item-id parameter(variable into item class) so that items could be recognise if they are the same object(used in stacking items in inv, copies of the same scriptable object aren't he same) - need to change code in some places to use this new variable - code could be cleaner - this solution solves few issues from the past but i don't exactly remeber where this  issue occure 

- Think on animations layers - could use/play few aniamtions at the same time (lower body is moving while upper is attacking etc???)
- Think on seperating Inventories script to few smaller scripts
- Think on a way to handle destroyinh item option menu when mouse was pressed outisde of dropdown, existing system isn't ideal


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
- UI scaling, I used anchors and some parts of the UI scales correctly with the screen but some parts(known item info, player lvl, player exp bar) overlap on each oder, also dynamic size of item info isn't working alwayes(maybe i determin size in pixels and should determin some percent of screen size?) 

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
-overall optimization
