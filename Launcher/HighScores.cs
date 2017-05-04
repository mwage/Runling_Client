using UnityEngine;

public class HighScores : MonoBehaviour {

    //0 = Game, 1-13 = levels, 14 = combined
    public static int[] highScoreSLA = new int[15];


    //load old scores
    private void Awake()
    {
        highScoreSLA[0] = PlayerPrefs.GetInt("HighScoreSLAGame");
        highScoreSLA[14] = PlayerPrefs.GetInt("HighScoreSLACombined");
        for (int i = 1; i < (highScoreSLA.Length - 1); i++)
        {
            highScoreSLA[i] = PlayerPrefs.GetInt("HighScoreSLA" + i.ToString());
        }
        
    }   
}
