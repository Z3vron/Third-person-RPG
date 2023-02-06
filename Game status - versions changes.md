I started learning Unity Game Engine by creating my first game which of course isn't perfect (not even acceptably good but i have lots of fun in process of making it).
This is my game - there are lots of bugs and issues with it but i am slowly repeairing them and adding some new features. Game doesn't look to good visualy but i never 
intended to make visually pleasing game - I am not an artist but i like coding so i focus on writing mechanics for the game.
I added some models and animations from the store (ofcourse free assets because i am student) so that game will look more visually pleasing

Think about
- Use profiler for both performance and garbage
- Think on Gc - could use struct rather than classes because struct doesn't create as mach garbage especially when i need container for simple type of data,for simple data eg. no arrayes,lists etc structs should be better, need to check profiler for the Garbage collector allocation, need to check If scriptable objects create garbage - where they are alocated
- Addidng strings create small amount on garbage - i am not using it every frame but might be an option to change in the future
- Debug.log creates lots of garbage ok while working on the game but before building game delete/comment all debug.log lines
- Think on reusing objects specially UI elemetns right now i create new instances of original prefab and later destroy them but i could turn on prefab in new desired position then i would need to reset prefab to riginal form
- Use get-set (for example get returns value and sets change it if condition was met)
- Move checking input flags from movement script to input handler - earlier there was a problem with it but i dont remember what was it 
- Work on and reduce lines in code about transfering items i have separate function that calls others (i use it in the shortcut and item option) but i also have old code that i use for the tranfer while dragging item with mouse - there are some diffrence i think but I'm not sure - need to thoroughly read code and delete one part
- Camera shakes on stairs 
- some things i do to often (each frame etc) should be done less time
- delegates in C#, pointers or their counterparts use var => var.something etc
- Do I want to be possible to put all weapons to both enemies and player hands? it creates some mess in code
- Same button for tap and hold - some issues is it even possible?
- I added game manager but i am not sure what data/references to put inside and where to use them because i am afraid that i will use it everywhere - right now i store player scripts like inventory etc but i use it in only few places i could change it to use it everywhere i called it bot not sure

- Using game manager to access some values in scripts(specially ones related to player ) or use observer pattern(events)

Next upddate:
- Fixed GUI issue with showing weapon poison
- weapon loss all efects when putted in inventory
- Added logic and GUI for player effects
- Added boosting dmg potion

Update 09.01.2023
- Added event action to handle player death so that code would be more clean and there would be less references between scripts
- Changed enemy death handler so that it would be less often checked - i disable script when enemy or player die so that update function won't be called - not sure is there better way
- Started working on redoing Invenotires script - clearing extra unuseful code and rewriting it to be more easly understand and less complicated - oh boy what a spaghetti code i have made if someone would try to understand it than i can only wish that person good luck
- I created base class for inventory slots making it more general but than original classes were reduced to just store variables that i use from the inventories script - I am not sure if this is the best solution - originally i just pass an argument(item) to function in class and within func body it would choose on which variable it should work - now i require to pass exact variable(list) into function not sure which solution is better
- Moved updating UI elements from update to when they are changed
- Added blocking attacks - some issues with consistency for both shield in left hand and weapon in right hand
- Added parrying attacks - but still issues when enemy attack is registered sometimes it just goes through player 
- Started using static Game manager to connect some scripts - think about it  
- Fixed issue when player would still be in lock on mode even that he shouldn't
- Fixed issue when player wouldn't lock on enemy because of the small obstacle between them
- Change updating UI so that it is separated from the logic - used C# Events(observer pattern) - with player UI no issue but with enemy there was a problem that event was called on all enemies HUD in world canvas so i pass specifc one to check - not most efficientAdded game mangaer but i am still not sure where i could use it


Update 14.11.2022
- Fixed bug when player was stuck at ceiling after hitting it
- Worked on cleaning code and moving some part of it to the other script so that each script would have one specific porpouse
- Reworked movement system so that player could not control movement while in the air, now i am using only one character controler move func - this will allow for good player speed measurement - by doing this i also made code more clean and removed unnecessary variables and lines of code, changed animations transisions so that they would be more acurate to the actual player speed, add lerping to design speed
- Fixed bug when dashes wouldn't correctly start or would be stopped in the middle of animation( fixed issue when active animation was restarted to false to early)
- Fixed issues with interacting with objects while player was in few objects triggers areas
- fixed issue of shaking camera and player trying to go up/down while going front/backwards still there is an issue with shaking camera while walking on stairs
- Changed back option to move while in the air - player can move character in each direction but at decresed speed while in the air
- Fixed issue with dashing - i was using bool from the another script which didn't update on time so i needed to changed it to direct get value from the animator
- Fixed bug when player could push enemy just by going into them



Update 25.10.2022:
- player strong attack interrupt enemy attack( added animations for both player and enemy)
- issue with humanoid animations on generic rig - changing type gives errores
- complete adding animations for first enemy( still missing some nice attack animations)
- Berries are now gathered from bushes
- Added quick potions slots(similar to weapon slots) - equiping, removing from slots and using in all methods(shortcut,drag,item menu option)
- Changed playing sound from the player - still needs to learn about sound in unity because i dont know nothing about it
- Added window pop up when adding/removing items from the player inventory( one window - so if many diffrent items are moved at the same time for example while creating item then window won't show it properly)


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
