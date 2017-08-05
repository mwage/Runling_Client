using System.Collections;
using Launcher;
using Players;
using RLR.Levels;
using TMPro;
using UnityEngine;

namespace RLR
{
    public class DeathRLR : MonoBehaviour
    {
        //events following Deathtrigger
        public void Death(LevelManagerRLR manager, InitializeGameRLR initializeGame, PlayerManager playerManager)
        {
            playerManager.CheckIfDead = false;
            playerManager.IsImmobile = true;
            playerManager.CheckIfDead = false;
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(false);
            playerManager.CharacterController.DisableAllSkills();


            switch (GameControl.GameState.SetGameMode)
            {
                case GameMode.Classic:
                    manager.EndGame(1);
                    break;
                case GameMode.Practice:
                    StartCoroutine(Respawn(3, 3, initializeGame, playerManager));
                    break;
                case GameMode.TimeMode:
                    if (playerManager.Lives == 0)
                    {
                        manager.EndGame(1);
                        break;
                    }
                    else
                    {
                        playerManager.Lives -= 1;
                        manager.LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + playerManager.Lives;
                        StartCoroutine(Respawn(10, 3, initializeGame, playerManager));
                        break;
                    }
            }
        }

        private static IEnumerator Respawn(int countdownFrom, float shieldDuration, InitializeGameRLR initializeGame, PlayerManager playerManager)
        {
            yield return new WaitForSeconds(1);

            // Countdown
            var respawnIn = Instantiate(initializeGame.CountdownPrefab, GameObject.Find("Canvas").transform);
            respawnIn.GetComponent<TextMeshProUGUI>().text = "Respawn in";
            respawnIn.GetComponent<TextMeshProUGUI>().fontSize = 30;
            respawnIn.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 100);

            for (var i = 0; i < countdownFrom; i++)
            {
                var countdown = Instantiate(initializeGame.CountdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (countdownFrom - i).ToString();
                if (i == countdownFrom - 1)
                {
                    playerManager.Model.SetActive(true);
                    playerManager.Shield.SetActive(true);
                    playerManager.IsDead = false;
                    playerManager.IsInvulnerable = true;
                    playerManager.CheckIfDead = true;
                    
                }
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }
            Destroy(respawnIn);
            
            playerManager.IsImmobile = false;

            yield return new WaitForSeconds(shieldDuration);

            playerManager.Shield.SetActive(false);
            playerManager.IsInvulnerable = false;
        }
    }
}
