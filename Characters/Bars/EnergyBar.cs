using Launcher;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Bars
{
    public class EnergyBar : MonoBehaviour
    {
        private Slider _energySlider;
        private Text _energyText;

        private void Awake()
        {
            _energySlider = gameObject.GetComponentInChildren<Slider>();
            _energyText = gameObject.GetComponentInChildren<Text>();
        }

        public void SetText(string text)
        {
            _energyText.text = text;
        }

        public void SetProgress(float maxEnergyFraction)
        {
            _energySlider.value = maxEnergyFraction;
        }
    }
}

