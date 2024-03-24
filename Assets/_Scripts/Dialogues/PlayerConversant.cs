using UnityEngine;

namespace RPG.Dialogues
{
    public class PlayerConversant : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private DialogueSO _currentDialogue;


        // Methods
        public string GetText()
        {
            if (_currentDialogue == null)
            {
                return "";
            }

            return _currentDialogue.RootNode.GetText();
        }
    }
}
