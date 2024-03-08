using System;
using TMPro;
using UnityEngine;

namespace RPG.Stats
{
    public class ExperienceText : MonoBehaviour
    {
        private Experience experience;
        private TextMeshProUGUI experienceValueText;

        private void Awake()
        {
            experience = GameObject.FindGameObjectWithTag("Player").GetComponent<Experience>();
            experienceValueText = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            experienceValueText.text = String.Format("Experience: {0:0}", experience.ExperiencePoint);
        }
    }
}
