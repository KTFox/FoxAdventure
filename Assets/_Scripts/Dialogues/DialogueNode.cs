using UnityEngine;

namespace RPG.Dialogues
{
    [System.Serializable]
    public class DialogueNode
    {
        public string UniqueId;
        public string Text;
        public string[] ChildrenDialogues;
        public Rect Rect = new Rect(0, 0, 200, 100);
    }
}
