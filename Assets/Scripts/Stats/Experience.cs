using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        // Variables

        [SerializeField]
        private float _experiencePoints;

        // Properties

        public float ExperiencePoint => _experiencePoints;

        // Events

        public event Action OnExperienceGained;


        // Methods

        public void GainExperience(float experience)
        {
            _experiencePoints += experience;
            OnExperienceGained?.Invoke();
        }

        #region ISaveable interface implements
        object ISaveable.CaptureState()
        {
            return _experiencePoints;
        }

        void ISaveable.RestoreState(object state)
        {
            _experiencePoints = (float)state;
        }
        #endregion
    }
}
