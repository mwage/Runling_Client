using Launcher;
using SLA;
using TMPro;
using UnityEngine;

namespace UI.SLA_Menus
{
    public class HighScoreMenuSLA : MonoBehaviour {

        public GameObject Menu;
        public GameObject ScorePrefab;
        public GameObject Background;
        public ControlSLA ControlSLA;
        public ScoreSLA Score;

        private GameObject _descriptionText;
        private readonly GameObject[] _levelScore = new GameObject[LevelManagerSLA.NumLevels];
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

            if (Score == null || GameControl.GameState.SetGameMode == GameMode.Practice)
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
            // Show highscores
            for (var i = 0; i < LevelManagerSLA.NumLevels; i++)
            {
                _levelScore[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = GameControl.HighScores.HighScoreSLA[i + 1].ToString();
            }

            _gameScore.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = GameControl.HighScores.HighScoreSLA[0].ToString();
            _combinedScore.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = GameControl.HighScores.HighScoreSLA[14].ToString();

            // Current game scores
            if (Score != null && GameControl.GameState.SetGameMode != GameMode.Practice)
            {
                for (var i = 0; i < LevelManagerSLA.NumLevels; i++)
                {
                    _levelScore[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Score.LevelScoreCurGame[i].ToString();

                    // New records are shown green
                    if (Score.LevelScoreCurGame[i] >= GameControl.HighScores.HighScoreSLA[i + 1] && Score.LevelScoreCurGame[i] != 0)
                    {
                        _levelScore[i].transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.green;
                    }
                }

                _gameScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = ControlSLA.PlayerManager.TotalScore.ToString();
                if (ControlSLA.PlayerManager.TotalScore > GameControl.HighScores.HighScoreSLA[0] && ControlSLA.PlayerManager.TotalScore > 0)
                {
                    _gameScore.transform.GetChild(2).GetComponent<TextMeshProUGUI>().color = Color.green;
                }
                _combinedScore.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        public void Back()
        {
            gameObject.SetActive(false);
            Menu.gameObject.SetActive(true);
        }
    }
}
