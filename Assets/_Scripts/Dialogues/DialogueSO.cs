using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private List<DialogueNode> _dialogueNodes;

        // Properties

        public IEnumerable<DialogueNode> DialogueNodes => _dialogueNodes;
        public DialogueNode RootNode => _dialogueNodes[0];


        // Methods

#if UNITY_EDITOR
        private void Awake()
        {
            if (_dialogueNodes.Count == 0)
            {
                _dialogueNodes.Add(new DialogueNode());
            }
        }
#endif
    }
}
