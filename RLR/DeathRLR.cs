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
            GameControl.Instance.State.IsImmobile = true;
            GameControl.Instance.State.Player.GetComponent<PlayerMovement>().IsAutoClicking = false;
            GameControl.Instance.State.Player.SetActive(false);


            if (GameControl.Instance.State.SetGameMode == Gamemode.Classic)
            {
                manager.EndGame(0.1f);
            }
            else if (GameControl.Instance.State.SetGameMode == Gamemode.Practice)
            {
                StartCoroutine(Respawn(1, initializeGame, control));
            }
        }

        IEnumerator Respawn(float delay, InitializeGameRLR initializeGame, ControlRLR control)
        {
            yield return new WaitForSeconds(2f);
            GameControl.Instance.State.Player.SetActive(true);
            GameControl.Instance.State.Player.transform.Find("Shield").gameObject.SetActive(true);
            GameControl.Instance.State.IsInvulnerable = true;
            GameControl.Instance.State.IsDead = false;
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

            GameControl.Instance.State.IsImmobile = false;
            yield return new WaitForSeconds(delay);
            GameControl.Instance.State.Player.transform.Find("Shield").gameObject.SetActive(false);
            GameControl.Instance.State.IsInvulnerable = false;
        }
    }
}
