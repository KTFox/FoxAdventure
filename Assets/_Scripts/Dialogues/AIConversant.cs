using UnityEngine;
using RPG.Control;

namespace RPG.Dialogues
{
    public class AIConversant : MonoBehaviour, IRaycastable
    {
        // Variables

        [SerializeField]
        private DialogueSO _dialogueSO;


        // Methods

        #region IRaycastable implements
        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController playerController)
        {
            if (_dialogueSO == null)
            {
                return false;
            }

            if (Input.GetMouseButton(0))
            {
                playerController.GetComponent<PlayerConversant>().StartDialogue(_dialogueSO);
            }

            return true;
        }
        #endregion
    }
}
