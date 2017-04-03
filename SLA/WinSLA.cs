using Assets.Scripts.Launcher;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.SLA
{
    public class WinSLA : MonoBehaviour
    {
        public Text hSTextSLA1;
        public Text hSTextSLA2;
        public Text hSTextSLA3;
        public Text hSTextSLA4;
        public Text hSTextSLA5;
        public Text hSTextSLA6;
        public Text hSTextSLA7;
        public Text hSTextSLA8;
        public Text hSTextSLA9;
        public Text hSTextSLA10;
        public Text hSTextSLA11;
        public Text hSTextSLA12;
        public Text hSTextSLA13;

        public Text hSTextSLAGame;
        public Text hSTextSLACombined;

        public void Awake()
        {
            hSTextSLA1.text = HighScoreSLA.highScoreSLA[1].ToString();
            hSTextSLA2.text = HighScoreSLA.highScoreSLA[2].ToString();
            hSTextSLA3.text = HighScoreSLA.highScoreSLA[3].ToString();
            hSTextSLA4.text = HighScoreSLA.highScoreSLA[4].ToString();
            hSTextSLA5.text = HighScoreSLA.highScoreSLA[5].ToString();
            hSTextSLA6.text = HighScoreSLA.highScoreSLA[6].ToString();
            hSTextSLA7.text = HighScoreSLA.highScoreSLA[7].ToString();
            hSTextSLA8.text = HighScoreSLA.highScoreSLA[8].ToString();
            hSTextSLA9.text = HighScoreSLA.highScoreSLA[9].ToString();
            hSTextSLA10.text = HighScoreSLA.highScoreSLA[10].ToString();
            hSTextSLA11.text = HighScoreSLA.highScoreSLA[11].ToString();
            hSTextSLA12.text = HighScoreSLA.highScoreSLA[12].ToString();
            hSTextSLA13.text = HighScoreSLA.highScoreSLA[13].ToString();

            hSTextSLAGame.text = HighScoreSLA.highScoreSLA[0].ToString();
            hSTextSLACombined.text = HighScoreSLA.highScoreSLA[14].ToString();
        }

        public void BackToMenu()
        {
            GameControl.gameActive = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void RestartGame()
        {
            GameControl.gameActive = true;
            GameControl.dead = true;
            HighScoreSLA.totalScoreSLA = 0;
            GameControl.currentLevel = 0;

            SceneManager.LoadScene("SLA");
        }
    }
}