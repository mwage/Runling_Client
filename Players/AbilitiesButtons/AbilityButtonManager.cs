using UnityEngine;
using UnityEngine.UI;


namespace Players.AbilitiesButtons
{
    public class AbilityButtonManager : MonoBehaviour
    {
        public GameObject Ability1Button;
        public GameObject Ability2Button;
        public GameObject Ultimatum;

        private AbilityButton _ability1, _ability2, _ultimatum;


        public void InitializeAbilityButtons(PlayerManager playerManager)
        {
            _ability1 = new AbilityButton(Ability1Button.GetComponent<Slider>(), "1", playerManager);
            _ability2 = new AbilityButton(Ability2Button.GetComponent<Slider>(), "2", playerManager);
            _ultimatum = new AbilityButton(Ultimatum.GetComponent<Slider>(), "Ultimatum", playerManager);
        }

        public void LateUpdate()
        {
            _ability1.SetProgress();
            _ability2.SetProgress();
            _ultimatum.SetProgress();
        }
    }
}