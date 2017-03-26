# Runling

### Script folder structure:

Launcher (Folder): scripts that won't be destroyed on load to hold important variables for different modes
 * GameControl: static variables: dead, gameActive, currentLevel, moveSpeed, lastLevelSLA
 * HighScoreSLA: static variables: totalScore, highScoreSLA[], loads highScoreSLA from playerprefs
 * SettingsSLA: moveSpeed[] for different levels
    
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
  * InitializeGameSLA: spawnimmunity, sets movespeed, countdown, leveltexts, instantiates player and activates deathtrigger
  * DeathSLA: manages what happens after death in SLA
  * ScoreSLA: handles the score (current/total), compares and sets highscores and triggers on highscore events
  * WinSLA: winscreen, shows highscores, winscreen buttons
  * StopCoroutineSLA: stops all methods that spawn regularly spawn drones when a new level is initialized
  * Levels (Folder): everything that happens during the levels (mostly related to drones)
    * LevelManager: Loads the scripts according to the level, ends the levels
    * Level1SLA: spawns drones for Level 1
    * Level2SLA: spawns drones for Level 2
    * Level3SLA: spawns drones for Level 3
    * Level4SLA: spawns drones for Level 4
    * Level5SLA: spawns drones for Level 5
    * BoundariesSLA: sets the area in which random drones get spawned
  * test (Folder): scripts related to test scenes
    * TestSLA: the control script for the drone test level
    * DroneTestSLA: loads drones for the test scene
    * ScoreTestSLA: handles score in the test scene
    * MovementTestSLA: script to test movements in the movementtest scene

Players (Folder): everything regarding the player
  * PlayerMovement: movement script
  * DeathTrigger: on trigger, sets player to dead
  * CameraMovement: moves camera

Drones (Folder): different kinds of drone movements
  * SpawnDrone: spawns drones at once: RandomBouncingDrone, RandomFlyingBouncingDrone, StraightFlyingOnewayDrone, StraightFlying360Drones
  * AddDrone: adds drones over time (coroutines): RandomBouncingDrone, RandomFlyingBouncingDrone
  * MoveDrone: adds force to make drones move: MoveStraight
  * DroneDirection: directions of drone: RandomDirection
  * DroneStartPosition: start positions: RandomPositionGround, RandomCornerGround, RandomPositionAir, RandomCornerAir
  * DestroyDroneTrigger: destroys oneway drones after hitting air collider
