using System.IO;
using Characters.Types;
using Launcher;
using TrueSync;
using UnityEngine;

namespace MP.TSGame.Players
{
    public class PlayerFactory : TrueSyncBehaviour
    {
        public GameObject ManticorePrefab;

        public GameObject Create(CharacterDto character, TSPlayerInfo player = null)
        {
            switch (character.Character)
            {
                case "Manticore":
                {
                    var playerObj = PhotonNetwork.Instantiate(Path.Combine("Characters", "Manticore"), Vector3.zero, Quaternion.identity, 0);
                    playerObj.transform.SetParent(transform);
//                    playerObj.GetComponentInChildren<PlayerTrigger>().InitializeTrigger();
                    GameControl.PlayerState.CharacterController = playerObj.AddComponent<Manticore>();
                    GameControl.PlayerState.CharacterController.Initialize(character);

                    return playerObj;
                }
                case "Unicorn":
                {
                    var playerObj = PhotonNetwork.Instantiate(Path.Combine("Characters", "Cat"), Vector3.zero, Quaternion.identity, 0);
                    playerObj.transform.SetParent(transform);
                    GameControl.PlayerState.CharacterController = playerObj.AddComponent<Unicorn>();
                    GameControl.PlayerState.CharacterController.Initialize(character);

                    return playerObj;
                }
                case "Arena":
                {
                    var playerObj = TrueSyncManager.SyncedInstantiate(ManticorePrefab, TSVector.zero, TSQuaternion.identity);
                    playerObj.transform.SetParent(transform);
                    var playerManager = playerObj.GetComponent<PlayerManager>();
                    playerManager.SetOwner(player);
                    playerManager.CharacterController = playerObj.AddComponent<ArenaCharacter>();
                    playerManager.CharacterController.Initialize(character);

                    return playerObj;
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
