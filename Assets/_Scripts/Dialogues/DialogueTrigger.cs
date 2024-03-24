using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogues
{
    public class DialogueTrigger : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private string actionTrigger;
        [SerializeField]
        private UnityEvent OnTrigger;


        // Methods

        public void TriggerAction(string actionToTrigger)
        {
            if (actionToTrigger == actionTrigger)
            {
                OnTrigger?.Invoke();
            }
        }
    }
}
