using System.Collections;
using System.Collections.Generic;
using Characters.Types;
using Drones;
using Launcher;
using Players;
using RLR.Levels;
using UnityEngine;

namespace RLR
{
    public class PlayerFactory : MonoBehaviour
    {
        // class creates player gameobject in hierarchy
        // returns Player to GameControl.PlayerState.Player - // TODO maybe we should change names here?

        public Transform Player; // top position in hierarchy
        public GameObject DroneManager; // so initalize chasers

        public GameObject ManticorePrefab;
        public GameObject UnicornPrefab;
        

        private PlayerFactory()
        {
            
        }

        public GameObject Create(CharacterDto character,  int? playerId = null)
        {
            switch (character.Character)
            {
                case "Manticore":
                {
                    GameObject player = Instantiate(ManticorePrefab, Player);
                    GameControl.PlayerState.CharacterController = player.AddComponent<Manticore>();
                    GameControl.PlayerState.CharacterController.Initizalize(character);
                        return player;
                }
                case "Unicorn":
                {
                    GameObject player = Instantiate(UnicornPrefab, Player);
                    GameControl.PlayerState.CharacterController = player.AddComponent<Unicorn>(); // attach script, and initialize it
                    GameControl.PlayerState.CharacterController.Initizalize(character);

                        return player;
                }
                default:
                {
                    Debug.Log("you want create non-existed character");
                    return null;
                }
                    
            }
            
        }

        //public void InitializeTrigger() // to deletet
        //{
        //    Player.GetComponentInChildren<PlayerTrigger>().RunlingChaser =
        //        DroneManager.GetComponent<RunlingChaser>();
        //}
    }
}
