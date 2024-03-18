using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceText : MonoBehaviour
    {
        // Variables

        private Experience _experience;
        private TextMeshProUGUI _experienceValueText;

        
        // Methods

        private void Awake()
        {
            _experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            _experienceValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            _experienceValueText.text = String.Format("Experience: {0:0}", _experience.ExperiencePoint);
        }
    }
}
