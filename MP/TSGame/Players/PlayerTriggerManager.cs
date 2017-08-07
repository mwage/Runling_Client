using Characters;
using Launcher;
using RLR;
using RLR.Levels;
using UnityEngine;

namespace MP.TSGame.Players
{
    public class PlayerTriggerManager : MonoBehaviour
    {
        public RunlingChaser RunlingChaser;

        public bool IsSafeZoneVisitedFirstTime(GameObject currentSafeZone, out int currentSafeZoneIdx)
        {
            if (GameControl.MapState.SafeZones.Contains(currentSafeZone)) // always should contain
            {
                currentSafeZoneIdx = GameControl.MapState.SafeZones.IndexOf(currentSafeZone);
                if (GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx])
                    return false; // you have been here, no exp for you

                return true;
            }
            else
            {
                Debug.Log("you gave not a safe zone to this function");
                currentSafeZoneIdx = -1;
                return false;
            }
        }

        public void MarkVisitedSafeZone(int currentSafeZoneIdx)
        {
            GameControl.MapState.VisitedSafeZones[currentSafeZoneIdx] = true;
        }

        public void AddExp(int currentSafeZoneIdx)
        {
//            GameControl.PlayerState.CharacterController.AddExp(LevelingSystem.CalculateExp(currentSafeZoneIdx,
//                GameControl.GameState.CurrentLevel, GameControl.GameState.SetDifficulty,
//                GameControl.GameState.SetGameMode));
        }

        public void CreateOrDestroyChaserIfNeed(GameObject currentSafeZone)
        {
//            RunlingChaser.CreateOrDestroyChaserIfNeed(currentSafeZone);
        }
    }
}