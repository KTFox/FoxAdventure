using UnityEngine;

namespace RPG.Combat
{
    public class AggroGroup : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private Fighter[] _enemiesToAggroed;


        // Methods

        private void Start()
        {
            Activate(false);
        }

        public void Activate(bool shouldActivate)
        {
            foreach (Fighter fighter in _enemiesToAggroed)
            {
                CombatTarget combatTarget = fighter.GetComponent<CombatTarget>();
                if (combatTarget != null)
                {
                    combatTarget.enabled = shouldActivate;
                }

                fighter.enabled = shouldActivate;
            }
        }
    }
}
