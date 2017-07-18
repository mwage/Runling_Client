using System.Collections;
using System.Collections.Generic;
using System.IO;
using Characters.Types;
using Launcher;
using UnityEngine;

namespace RLR
{
    public class PlayerFactory : MonoBehaviour
    {
        // class creates player gameobject in hierarchy
        // returns Player to GameControl.State.Player - // TODO maybe we should change names here?

        public Transform Player; // top position in hierarchy

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
                    var player = PhotonNetwork.Instantiate(Path.Combine("Characters", "Manticore"), Vector3.zero, Quaternion.identity, 0);
                    player.transform.SetParent(Player);
                    GameControl.PlayerState.CharacterController = player.AddComponent<Manticore>();
                    GameControl.PlayerState.CharacterController.Initizalize(character);

                    return player;
                }
                case "Unicorn":
                {
                    var player = PhotonNetwork.Instantiate(Path.Combine("Characters", "Cat"), Vector3.zero, Quaternion.identity, 0);
                    player.transform.SetParent(Player);
                    GameControl.PlayerState.CharacterController = player.AddComponent<Unicorn>();
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
    }
}
