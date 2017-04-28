using UnityEngine;

public class HighScoreRLR : MonoBehaviour
{

    //0 = Game, 1-13 = levels, 14 = combined
    public static int[] highScoreRLR = new int[15];


    //load old scores
    private void Awake()
    {
        highScoreRLR[0] = PlayerPrefs.GetInt("HighScoreRLRGame");
        highScoreRLR[14] = PlayerPrefs.GetInt("HighScoreRLRCombined");
        for (int i = 1; i < (highScoreRLR.Length - 1); i++)
        {
            highScoreRLR[i] = PlayerPrefs.GetInt("HighScoreRLR" + i.ToString());
        }

    }
}