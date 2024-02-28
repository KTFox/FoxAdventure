using System;
using UnityEngine;
using RPG.Saving;

namespace RPG.Stats
{
    public class Experience : MonoBehaviour, ISaveable
    {
        public event Action OnExperienceGained;

        [SerializeField]
        private float _experiencePoints;

        public float ExperiencePoint
        {
            get
            {
                return _experiencePoints;
            }
        }

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
