using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Characters.Bars
{
    public class LevelBar : MonoBehaviour
    {
        private Slider _levelSlider;
        private Text _levelText;

        private void Awake()
        {
            _levelSlider = gameObject.GetComponentInChildren<Slider>();
            _levelText = gameObject.GetComponentInChildren<Text>();
        }

        public void SetText(int level)
        {
            _levelText.text = level.ToString();
        }

        public void SetProgress(int level, int exp)
        {
            //float val = (float) (exp - LevelingSystem.LevelExperienceCurve[level]) /
            //            (LevelingSystem.LevelExperienceCurve[level + 1] - LevelingSystem.LevelExperienceCurve[level]);
            _levelSlider.value = (float)(exp - LevelingSystem.LevelExperienceCurve[level-1]) / (LevelingSystem.LevelExperienceCurve[level] - LevelingSystem.LevelExperienceCurve[level-1]);
           // Debug.Log(string.Format("lvl slider vlaue {0}", val));
        }
    }

}