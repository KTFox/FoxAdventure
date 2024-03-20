using UnityEngine;

namespace RPG.Dialogues
{
    [System.Serializable]
    public class DialogueNode
    {
        public string UniqueID;
        public string Text;
        public string[] ChildrenUniqueIDs;
        public Rect Rect = new Rect(0, 0, 200, 100);
    }
}
