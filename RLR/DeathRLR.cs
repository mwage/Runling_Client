using System.Collections;
using Assets.Scripts.Launcher;
using Assets.Scripts.Players;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.RLR
{
    public class DeathRLR : MonoBehaviour
    {

        //events following Deathtrigger
        public void Death(LevelManagerRLR manager, InitializeGameRLR initializeGame, ControlRLR control)
        {
            GameControl.IsImmobile = true;
            GameControl.Player.GetComponent<PlayerMovement>().IsAutoClicking = false;
            GameControl.Player.SetActive(false);


            if (GameControl.SetGameMode == GameControl.Gamemode.Classic)
            {
                manager.EndGame(0.1f);
            }
            else if (GameControl.SetGameMode == GameControl.Gamemode.Practice)
            {
                StartCoroutine(Respawn(1, initializeGame, control));
            }
        }

        IEnumerator Respawn(float delay, InitializeGameRLR initializeGame, ControlRLR control)
        {
            yield return new WaitForSeconds(2f);
            GameControl.Player.SetActive(true);
            GameControl.Player.transform.Find("Shield").gameObject.SetActive(true);
            GameControl.IsInvulnerable = true;
            GameControl.IsDead = false;
            control.StopUpdate = false;


            // Countdown
            var respawnIn = Instantiate(initializeGame.CountdownPrefab, GameObject.Find("Canvas").transform);
            respawnIn.GetComponent<TextMeshProUGUI>().text = "Respawn in";
            respawnIn.GetComponent<TextMeshProUGUI>().fontSize = 30;
            respawnIn.GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 100);

            for (var i = 0; i < 3; i++)
            {
                var countdown = Instantiate(initializeGame.CountdownPrefab, GameObject.Find("Canvas").transform);
                countdown.GetComponent<TextMeshProUGUI>().text = (3 - i).ToString();
                yield return new WaitForSeconds(0.5f);
                Destroy(countdown);
            }
            Destroy(respawnIn);

            GameControl.IsImmobile = false;
            yield return new WaitForSeconds(delay);
            GameControl.Player.transform.Find("Shield").gameObject.SetActive(false);
            GameControl.IsInvulnerable = false;
        }
    }
}
