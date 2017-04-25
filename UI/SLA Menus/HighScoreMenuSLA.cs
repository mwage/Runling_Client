using Assets.Scripts.Launcher;
using Assets.Scripts.SLA;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.SLA_Menus
{
    public class HighScoreMenuSLA : MonoBehaviour {

        public GameObject Menu;
        public GameObject ScorePrefab;
        public GameObject Background;
        public ScoreSLA ScoreSLA;
        

        public bool HighScoreMenuActive;
        private GameObject _descriptionText;
        private GameObject[] _levelScore = new GameObject[LevelManagerSLA.NumLevels];
        private GameObject _gameScore;
        private GameObject _combinedScore;


        private void Awake()
        {
            CreateTextObjects(Background);
        }

        private void OnEnable()
        {
            SetNumbers();
        }

        public void CreateTextObjects(GameObject background)
        {


            // Descriptions
            _descriptionText = Instantiate(ScorePrefab, background.transform);
            _descriptionText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level";
            _descriptionText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Cur:";
            _descriptionText.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "PB:";
            _descriptionText.GetComponent<RectTransform>().sizeDelta = new Vector2(50, 20);
            for (var i = 0; i < 3; i++)
            {
                _descriptionText.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta = new Vector2(50, 20);
            }

            // Level Highscores
            for (var i = 0; i < LevelManagerSLA.NumLevels; i++)
            {
                _levelScore[i] = Instantiate(ScorePrefab, background.transform);
                _levelScore[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = (i + 1).ToString();
            }

            // Game Score
            _gameScore = Instantiate(ScorePrefab, background.transform);
            _gameScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Game";
            _gameScore.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);
            _gameScore.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(60, 20);

            // Combined Score
            _combinedScore = Instantiate(ScorePrefab, background.transform);
            _combinedScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Combined";
            _combinedScore.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 20);
            _combinedScore.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(80, 20);

            if (!GameControl.GameActive)
            {
                _descriptionText.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
                _descriptionText.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 15);
                _gameScore.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 15);
                _combinedScore.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition += new Vector2(0, 15);
                for (var i = 0; i < LevelManagerSLA.NumLevels; i++)
                {
                    _levelScore[i].transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition +=
                        new Vector2(0, 15);
                }
            }
        }

        public void SetNumbers()
        {
            // Show ´highscores
            for (var i = 0; i < LevelManagerSLA.NumLevels; i++)
            {
                _levelScore[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = HighScoreSLA.highScoreSLA[i + 1].ToString();
            }

            _gameScore.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = HighScoreSLA.highScoreSLA[0].ToString();
            _combinedScore.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = HighScoreSLA.highScoreSLA[14].ToString();

            // current game scores
            if (GameControl.GameActive)
            {
                for (var i = 0; i < LevelManagerSLA.NumLevels; i++)
                {
                    _levelScore[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ScoreSLA.LevelScoreCurGame[i].ToString();
                }

                _gameScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = GameControl.TotalScore.ToString();
                _combinedScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }
        

        public void Back()
        {
            HighScoreMenuActive = false;
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }
}
