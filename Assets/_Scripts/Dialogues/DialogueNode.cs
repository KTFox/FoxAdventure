namespace RPG.Dialogues
{
    [System.Serializable]
    public class DialogueNode
    {
        public string UniqueId;
        public string DialogueText;
        public string[] ChildrenDialogues;
    }
}
