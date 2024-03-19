using UnityEngine;

namespace RPG.Dialogues
{
    [CreateAssetMenu(menuName ="ScriptableObject/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private DialogueNode[] _dialogueNodes;
    }
}
