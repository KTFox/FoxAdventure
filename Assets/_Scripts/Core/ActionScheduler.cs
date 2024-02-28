using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        private IAction currentAction;

        /// <summary>
        /// Call currentAction.Cancel() and set currentAction = action
        /// </summary>
        /// <param name="action"></param>
        public void StartAction(IAction action)
        {
            if (currentAction == action) return;

            if (currentAction != null)
            {
                currentAction.Cancel();
            }

            currentAction = action;
        }

        /// <summary>
        /// Set ActionScheduler.currentAction = null
        /// </summary>
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
