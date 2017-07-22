using System.IO;
using Characters.Types;
using Launcher;
using Players;
using UnityEngine;

namespace Characters
{
    public class PlayerFactory : MonoBehaviour
    {
        private PlayerFactory()
        {
        }

        public GameObject Create(CharacterDto character)
        {
            switch (character.Character)
            {
                case "Manticore":
                {
                    var player = PhotonNetwork.Instantiate(Path.Combine("Characters", "Manticore"), Vector3.zero, Quaternion.identity, 0);
                    player.transform.SetParent(transform);
                    player.GetComponentInChildren<PlayerTrigger>().InitializeTrigger();
                    GameControl.PlayerState.CharacterController = player.AddComponent<Manticore>();
                    GameControl.PlayerState.CharacterController.Initialize(character);

                    return player;
                }
                case "Unicorn":
                {
                    var player = PhotonNetwork.Instantiate(Path.Combine("Characters", "Cat"), Vector3.zero, Quaternion.identity, 0);
                    player.transform.SetParent(transform);
                    GameControl.PlayerState.CharacterController = player.AddComponent<Unicorn>();
                    GameControl.PlayerState.CharacterController.Initialize(character);

                    return player;
                }
                case "Arena":
                {
                    var player = PhotonNetwork.Instantiate(Path.Combine("Characters", "Manticore"), Vector3.zero, Quaternion.identity, 0);
                    player.transform.SetParent(transform);
                    GameControl.PlayerState.CharacterController = player.AddComponent<ArenaCharacter>();
                    GameControl.PlayerState.CharacterController.Initialize(character);

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
