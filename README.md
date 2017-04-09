# Runling

### Script folder structure:

Launcher (Folder): scripts that won't be destroyed on load to hold important variables for different modes
 * GameControl: static variables: dead, gameActive, currentLevel, moveSpeed
 * HighScoreSLA: static variables: totalScore, highScoreSLA[], loads highScoreSLA from playerprefs
    
UI (Folder): UI related scripts
 * MainMenu (Folder): Main menu related scripts, main menu also accesses SLAMenu/HighscoreMenuSLA and OptionsMenu
   * MainMenuManager: always active when in main menu or a submenu of it, handles back with ESC
   * MainMenu: buttons of the main menu
   * SLAMenu: buttons of the sla submenu
   * MainMenuBackground: handles the background of the main menu
 * SLAMenu (Folder): Ingame menu in SLA, also accesses OptionsMenu
   * InGameMenuManagerSLA: ESC to open menu and navigate back in menus, function to close all open menus
   * InGameMenuSLA: buttons of the initial ingame menu
   * HighscoreMenuSLA: handles the highscores and back button, also accessed from main menu
 * OptionsMenu: buttons of the options menu
    
SLA (Folder): All scripts used specifically for SLA
  * ControlSLA: script that controls the flow of the game
  * InitializeGameSLA: spawnimmunity, countdown, leveltexts, instantiates player and activates deathtrigger
  * DeathSLA: manages what happens after death in SLA
  * ScoreSLA: handles the score (current/total), compares and sets highscores and triggers on highscore events
  * WinSLA: winscreen, shows highscores, winscreen buttons
  * StopCoroutineSLA: stops all methods that spawn regularly spawn drones when a new level is initialized
  * Levels (Folder): everything that happens during the levels (mostly related to drones)
    * LevelManager: Manages the levels, drone creation, and transactions between levels
    * ADrone: Abstract class for levels.  All levels need a movement speed and a way to create drones
    * Level*X*SLA: spawns drones for Level *X*
    * BoundariesSLA: defines the boundaries for SLA
  * test (Folder): scripts related to test scenes
    * TestSLA: the control script for the drone test level
    * DroneTestSLA: loads drones for the test scene
    * ScoreTestSLA: handles score in the test scene
    * MovementTest: script to test movement in the movementtest scene
    * MovementTest2: script to test movement on a sphere

Players (Folder): everything regarding the player
  * PlayerMovement: movement script
  * DeathTrigger: on trigger, sets player to dead
  * CameraMovement: moves camera

Drones (Folder): different kinds of drone movements
  * IDrone: Drone interface to create/configure drones
  * ADrone: Abstract drone class that all drone classes are derived from
  * RandomBouncingDrone: Drone class for random bouncing drones
  * RandomFlyingBouncingDrone: Drone class for random flying bouncing drones
  * StraightFlying360Drone: Drone class for straight flying 360 drones
  * StraightFlyingOneWayDrone: Drone class for straight flying one way drones
  * ChaserDrone: Drone class for chaser drones
  * MineDrone: Drone class for mine drones
  * GridDrones: Spawns drones in grid pattern
  * MineVariations: Different patterns for drones spawned by mine drones
  * DroneFactory: Primary point of interaction to spawn/add drones
  * DroneDirection: Contains a static method to generate a random direction
  * MoveDrone: Contains a static method to move a drone in a straight line
  * DroneStartPosition: Contains some static methods for various ways to get a starting position
  * DestroyDroneTrigger: destroys oneway drones after hitting air collider
