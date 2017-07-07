using System.Collections;
using System.Collections.Generic;
using Characters.Types;
using UnityEngine;

namespace RLR
{
    public class PlayerFactory : MonoBehaviour
    {
        // class creates player gameobject in hierarchy
        // returns Player to GameControl.State.Player - // TODO maybe we should change names here?

        public Transform Player; // position in hierarchy

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
                    player.AddComponent<Manticore>().Initizalize(character);
                    return player;
                }
                case "Unicorn":
                {
                    GameObject player = Instantiate(UnicornPrefab, Player);
                    player.AddComponent<Unicorn>().Initizalize(character);
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
