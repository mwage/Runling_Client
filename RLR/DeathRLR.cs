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
        public void Death(LevelManagerRLR manager, InitializeGameRLR initializeGame, ControlRLR control)
        {
            GameControl.State.IsImmobile = true;
            GameControl.State.Player.GetComponent<PlayerMovement>().IsAutoClicking = false;
            GameControl.State.Player.SetActive(false);


            switch (GameControl.State.SetGameMode)
            {
                case Gamemode.Classic:
                    manager.EndGame(1);
                    break;
                case Gamemode.Practice:
                    StartCoroutine(Respawn(3, 3, initializeGame, control));
                    break;
                case Gamemode.TimeMode:
                    if (GameControl.State.Lives == 0)
                    {
                        manager.EndGame(1);
                        break;
                    }
                    else
                    {
                        GameControl.State.Lives -= 1;
                        manager.LivesText.GetComponent<TextMeshProUGUI>().text = "Lives remaining: " + GameControl.State.Lives;
                        StartCoroutine(Respawn(10, 3, initializeGame, control));
                        break;
                    }
            }
        }

        private static IEnumerator Respawn(int countdownFrom, float shieldDuration, InitializeGameRLR initializeGame, ControlRLR control)
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
                    GameControl.State.Player.SetActive(true);
                    GameControl.State.Player.transform.Find("Shield").gameObject.SetActive(true);
                    GameControl.State.IsInvulnerable = true;
                    GameControl.State.IsDead = false;
                    control.StopUpdate = false;
                }
                yield return new WaitForSeconds(1);
                Destroy(countdown);
            }
            Destroy(respawnIn);
            
            GameControl.State.IsImmobile = false;
            yield return new WaitForSeconds(shieldDuration);
            GameControl.State.Player.transform.Find("Shield").gameObject.SetActive(false);
            GameControl.State.IsInvulnerable = false;
        }
    }
}
