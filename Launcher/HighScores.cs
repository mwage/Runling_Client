using UnityEngine;

namespace Launcher
{
    public class HighScores {

        //0 = Game, 1-13 = levels, 14 = combined
        public int[] HighScoreSLA = new int[15];

        public HighScores()
        {
            LoadHighscores();
        }
    
        //load old scores
        private void LoadHighscores()
        {
            HighScoreSLA[0] = PlayerPrefs.GetInt("HighScoreSLAGame");
            HighScoreSLA[14] = PlayerPrefs.GetInt("HighScoreSLACombined");
            for (var i = 1; i < (HighScoreSLA.Length - 1); i++)
            {
                HighScoreSLA[i] = PlayerPrefs.GetInt("HighScoreSLA" + i);
            }
        }
    }
}
