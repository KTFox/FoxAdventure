using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogues
{
    [System.Serializable]
    public class DialogueNode
    {
        public string UniqueID;
        public string Text;
        public List<string> ChildrenUniqueIDs = new List<string>();
        public Rect Rect = new Rect(0, 0, 200, 100);
    }
}
