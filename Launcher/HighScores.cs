using UnityEngine;

namespace Launcher
{
    public class HighScores {

        // 0 = Game, 1-13 = levels, 14 = combined
        public int[] HighScoreSLA = new int[15];

        // 0 = TimeMode score, 1-9 = levels
        public float[] HighScoreRLRNormal = new float[10];
        public float[] HighScoreRLRHard = new float[10];

        private const int Version = 2;

        public HighScores()
        {
            var reset = PlayerPrefs.GetInt("Reset");
            if (reset < Version)
            {
                Debug.Log("resetting scores");
                ResetHighscoresRLR();
                ResetHighscoresSLA();
                PlayerPrefs.SetInt("Reset", Version);
            }

            LoadHighscoresSLA();
            LoadHighscoresRLR();
        }
    
        //load old scores
        private void LoadHighscoresSLA()
        {
            HighScoreSLA[0] = PlayerPrefs.GetInt("HighScoreSLAGame");
            HighScoreSLA[14] = PlayerPrefs.GetInt("HighScoreSLACombined");
            for (var i = 1; i < (HighScoreSLA.Length - 1); i++)
            {
                HighScoreSLA[i] = PlayerPrefs.GetInt("HighScoreSLA" + i);
            }
        }

        private void LoadHighscoresRLR()
        {
            HighScoreRLRNormal[0] = PlayerPrefs.GetFloat("HighScoreRLRNormalTimeMode");
            HighScoreRLRHard[0] = PlayerPrefs.GetFloat("HighScoreRLRHardTimeMode");

            for (var i = 1; i < (HighScoreRLRNormal.Length); i++)
            {
                HighScoreRLRNormal[i] = PlayerPrefs.GetFloat("HighScoreRLRNormal" + i);
            }
            for (var i = 1; i < (HighScoreRLRHard.Length); i++)
            {
                HighScoreRLRHard[i] = PlayerPrefs.GetFloat("HighScoreRLRHard" + i);
            }
        }

        private void ResetHighscoresSLA()
        {
            PlayerPrefs.SetInt("HighScoreSLAGame", 0);
            PlayerPrefs.SetInt("HighScoreSLACombined", 0);
            for (var i = 1; i < (HighScoreSLA.Length - 1); i++)
            {
                PlayerPrefs.SetInt("HighScoreSLA" + i, 0);
            }
        }

        private void ResetHighscoresRLR()
        {
            PlayerPrefs.SetFloat("HighScoreRLRNormalTimeMode", 0);
            PlayerPrefs.SetFloat("HighScoreRLRHardTimeMode", 0);

            for (var i = 1; i < (HighScoreRLRNormal.Length); i++)
            {
                PlayerPrefs.SetFloat("HighScoreRLRNormal" + i, 0);
            }
            for (var i = 1; i < (HighScoreRLRHard.Length); i++)
            {
                PlayerPrefs.SetFloat("HighScoreRLRHard" + i, 0);
            }
        }
    }
}
