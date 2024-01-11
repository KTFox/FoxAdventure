using UnityEngine;

namespace RPG.Core {
    public class ActionScheduler : MonoBehaviour {

        private IAction currentAction;

        /// <summary>
        /// Set currentAction equal action and call Cancel() function
        /// </summary>
        /// <param name="action"></param>
        public void StartAction(IAction action) {
            if (currentAction == action) return;

            if (currentAction != null) {
                currentAction.Cancel();
            }

            currentAction = action;
        }

    }
}
