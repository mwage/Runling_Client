using DarkRift;
using Players;
using UnityEngine.UI;

namespace SLA
{
    public class ScoreDataSLA : IDarkRiftSerializable
    {
        public PlayerManager PlayerManager { get; }
        public int CurrentScore { get; set; }
        public int TotalScore { get; set; }
        public int[] ScoresCurrentGame { get; }
        public uint PlayerId { get; private set; }

        private readonly Text _currentScoreText;
        private readonly Text _totalScoreText;

        /// <summary>
        /// Initialize client-side
        /// </summary>
        public ScoreDataSLA(PlayerManager playerManager, int numberOfLevels, Text currentScoreText, Text totalScoreText)
        {
            PlayerManager = playerManager;
            ScoresCurrentGame = new int[numberOfLevels];
            _currentScoreText = currentScoreText;
            _totalScoreText = totalScoreText;
            SetText();
        }
        
        /// <summary>
        /// Initialize server-side
        /// </summary>
        public ScoreDataSLA(PlayerManager playerManager, int numberOfLevels)
        {
            PlayerManager = playerManager;
            ScoresCurrentGame = new int[numberOfLevels];
        }

        /// <summary>
        /// For serialization
        /// </summary>
        public ScoreDataSLA()
        {
        }

        public void IncrementScore(int increment)
        {
            CurrentScore += increment;
            TotalScore += increment;
            SetText();
        }

        public void UpdateScore(int currentScore, int totalScore)
        {
            CurrentScore = currentScore;
            TotalScore = totalScore;
            SetText();
        }

        public void ResetCurrent()
        {
            CurrentScore = 0;
            SetText();
        }

        public void SetSiblingIndex(int index)
        {
            _currentScoreText?.transform.parent.SetSiblingIndex(index + 1);
        }

        private void SetText()
        {
            if (_currentScoreText == null || _totalScoreText == null)
                return;

            _currentScoreText.text = CurrentScore.ToString();
            _totalScoreText.text = TotalScore.ToString();
        }

        public void Deserialize(DeserializeEvent e)
        {
            PlayerId = e.Reader.ReadUInt32();
            CurrentScore = e.Reader.ReadUInt16();
            TotalScore = e.Reader.ReadUInt16();
        }

        public void Serialize(SerializeEvent e)
        {
            e.Writer.Write(PlayerManager.Player.Id);
            e.Writer.Write((ushort)CurrentScore);
            e.Writer.Write((ushort)TotalScore);
        }
    }
}
