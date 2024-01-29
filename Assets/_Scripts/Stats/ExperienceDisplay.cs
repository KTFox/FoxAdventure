using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats {
    public class ExperienceDisplay : MonoBehaviour {

        private Experience experience;
        private TextMeshProUGUI experienceValueText;

        private void Awake() {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            experienceValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update() {
            experienceValueText.text = String.Format("{0:0}", experience.GetExperiencePoint());
        }
    }
}
