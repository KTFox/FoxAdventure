using UnityEngine;

namespace RPG.Core
{
    public class ActionScheduler : MonoBehaviour
    {
        // Variables

        private IAction _currentAction;


        // Methods

        /// <summary>
        /// Cancel current action and set new action
        /// </summary>
        /// <param name="action"></param>
        public void StartAction(IAction action)
        {
            if (_currentAction == action) return;

            if (_currentAction != null)
            {
                _currentAction.Cancel();
            }

            _currentAction = action;
        }

        /// <summary>
        /// Set ActionScheduler._currentAction = null
        /// </summary>
        public void CancelCurrentAction()
        {
            StartAction(null);
        }
    }
}
