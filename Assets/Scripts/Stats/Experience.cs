using RPG.Saving;
using System;
using UnityEngine;

namespace RPG.Stats {
    public class Experience : MonoBehaviour, ISaveable {

        public event Action onExperienceGained;

        [SerializeField]
        private float experiencePoints;

        public void GainExperience(float experience) {
            experiencePoints += experience;
            onExperienceGained();
        }

        public float GetExperiencePoint() {
            return experiencePoints;
        }

        #region ISaveable interface implements
        public object CaptureState() {
            return experiencePoints;
        }

        public void RestoreState(object state) {
            experiencePoints = (float)state;
        }
        #endregion
    }
}