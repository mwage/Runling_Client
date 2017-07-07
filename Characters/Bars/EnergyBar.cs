using UnityEngine;
using UnityEngine.UI;

namespace Characters.Bars
{
    public class EnergyBar : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        private Slider _energySlider;
        private Text _energyText;

        private void Awake()
        {
            _energySlider = gameObject.GetComponentInChildren<Slider>();
            _energyText = gameObject.GetComponentInChildren<Text>();
        }

        public void SetText(int level)
        {
            _energyText.text = level.ToString();
        }

        public void SetProgress(int level, int exp)
        {
            _energySlider.value = (float)(exp - LevelingSystem.LevelExperienceCurve[level]) / (LevelingSystem.LevelExperienceCurve[level + 1] - LevelingSystem.LevelExperienceCurve[level]);
        }
    }
}

