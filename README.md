# Runling

### Script folder structure:

Launcher (Folder): scripts that won't be destroyed on load to hold important variables for different modes
 * GameControl: static variables you always want to have access to (f.e.: dead, currentLevel, moveSpeed, cameraSettings, etc.)
 * HighScoreSLA: loads SLA Highscore from playerprefs
 * InputManager: handles player input and hotkeys
    
UI (Folder): UI related scripts
 * MainMenu (Folder): Main Menu related scripts
   * MainMenuManager: always active when in Main Menu or a submenu of it, handles back with ESC
   * MainMenu: Main Menu
   * SLAMenu: SLA mode selection and Highscores
   * RLRMenu: RLR Difficulty and mode selection
   * BackgroundSLA: left side of the background of the Main Menu
   * BackgroundRLR: right sinde of the background of the Main Menu
 * SLAMenu (Folder): Ingame menu in SLA
   * InGameMenuManagerSLA: ESC to open menu and navigate back in menus, function to close all open menus
   * InGameMenuSLA: buttons of the initial ingame menu
   * HighscoreMenuSLA: Highscore Menu, also accessed from Main Menu
   * ChooseLevelMenuSLA: Menu for to choose your level in practice mode
   * WinSLA: winscreen, shows highscores, winscreen buttons
  * RLRMenu (Folder): Ingame menu in RLR
   * InGameMenuManagerRLR: ESC to open menu and navigate back in menus, function to close all open menus
   * InGameMenuRLR: buttons of the initial ingame menu
   * HighscoreMenuRLR: currently not used, will be rewritten for time mode
   * ChooseLevelMenuRLR: Menu for to choose your level in practice mode
 * OptionsMenu (Folder): scripts for the options menu
  * OptionsMenu: Options Menu 
  * SetHotkeys: rebinding of hotkeys
  * SetCamera: camera settings
  * SubmenuBuilder: Creates the buttons/sliders for the Options Menu submenus
 * SceneLoader: Loads scene in the background
    
Drones (Folder): different kinds of drone movements  
  * DroneFactory: Primary point of interaction to spawn/add drones
  * DroneStartPosition: StartPositionDelegate, static methods to get specific start positions
  * DroneDirection: Contains static method to generate random direction
  * DestroyDroneTrigger: destroys oneway drones after hitting air collider
  * MineVariations: the synced 360 mines
  * Pattern (Folder): 
    * IPattern: interface for pattern
    * APattern: Abstract class all pattern classes are derived from
    * Pat360Drone: Spawns drones going out in a circle, with a lot of parameters available
    * PatGridDrones: Spawns drones in a grid pattern
    * PatContinuousSpawn: keeps spawning drones
  * DroneTypes (Folder): different types of drones
    * IDrone: Drone interface to create/configure drones
    * ADrone: Abstract drone class that all drone classes are derived from
    * DefaultDrone: drone that takes direction and position as parameters
    * RandomDrone: drone that randomizes position and direction according to StartPositionDelegate
    * MineDrone: Drone class for mine drones
    * RedDrone: Red drones from RLR, choose random direcion, move there, then rotate in new direction
  * Movement (Folder): different movement types for drones
    * DroneMovement: Contains movement delegates, adds the according movementscripts to the drones
    * ChaserMovement: drone follows the player
    * CurvedMovement: drone goes in a circle (radius dependant on parameters)
    * SinusoidalMovement: drone moves in a sin pattern
    * PointToPointMovement: movement for red drones from RLR
    
Players (Folder): everything regarding the player
  * PlayerMovement: movement script
  * PlayerTrigger: handles various trigger events
  * CameraMovement: moves camera, sets camera according to camera settings
  * MovementTest: script to test movement in the movementtest scene

SLA (Folder): All scripts used specifically for SLA
  * ControlSLA: script that controls the flow of the game
  * InitializeGameSLA: spawnimmunity, countdown, leveltexts, instantiates player and activates deathtrigger
  * DeathSLA: manages what happens after death in SLA
  * ScoreSLA: handles the score (current/total), compares and sets highscores and triggers on highscore events
  * Levels (Folder): everything that happens during the levels (mostly related to drones)
    * LevelManager: Manages the levels, drone creation, and transactions between levels
    * ILevelSLA: Interface for SLA levels
    * ALevelSLA: Abstract class for SLA levels.
    * Level*X*SLA: spawns drones for Level *X*
    * BoundariesSLA: defines the boundaries for drones in SLA

RLR (Folder): All scripts used specifically for RLR
  * ControlRLR: script that controls the flow of the game
  * InitializeGameRLR: spawnimmobility, countdown, leveltexts, instantiates player
  * DeathRLR: manages what happens after death in RLR
  * WinRLR: win screen for RLR
  * Levels (Folder): everything that happens during the levels (mostly related to drones)
    * LevelManager: Manages the levels, drone creation, and transactions between levels
    * ILevelRLR: Interface for RLR levels
    * ALevelRLR: Abstract class for RLR levels
    * Runling Chaser: sets and manages what platforms spawn chaser drones
    * Normal (Folder): Normal difficulty levels
    * Hard (Folder): Hard difficulty levels
  * GenerateMap (Folder): Generates RLR spiral map
    * GenerateMap: Generates spawn parameters for the map parts, sets colliders
    * ALane: Abstract class for prefabs, adjusts all parameters for the prefab (scaling mostly)
    * Lanes: scaling for special lanes (f.e. Last lane)


