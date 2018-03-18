using Client.Scripts.Launcher;
using Game.Scripts.GameSettings;
using Game.Scripts.Players;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Client.Scripts.RLR
{
    public class DeathRLR : MonoBehaviour
    {
        private InitializeGameRLR _initializeGame;
        private LevelManagerRLR _levelManager;

        private void Awake()
        {
            _initializeGame = GetComponent<InitializeGameRLR>();
            _levelManager = GetComponent<LevelManagerRLR>();

            PlayerTrigger.onPlayerDeath += Death;
        }

        private void OnDestroy()
        {
            PlayerTrigger.onPlayerDeath -= Death;
        }

        //events following Deathtrigger
        public void Death(PlayerManager playerManager)
        {
            playerManager.IsImmobile = true;
            playerManager.IsInvulnerable = true;
            playerManager.Model.SetActive(false);
            playerManager.CharacterManager.DisableAllSkills();


            switch (GameControl.GameState.SetGameMode)
            {
                case GameMode.Classic:
                    _levelManager.EndGame(1);
                    break;
                case GameMode.Practice:
                    StartCoroutine(Respawn(3, 3, _initializeGame, playerManager));
                    break;
                case GameMode.TimeMode:
                    if (playerManager.Lives == 0)
                    {
                        _levelManager.EndGame(1);
                        break;
                    }
                    else
                    {
                        _initializeGame.ChangeLives(playerManager, playerManager.Lives - 1);
                        StartCoroutine(Respawn(10, 3, _initializeGame, playerManager));
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
