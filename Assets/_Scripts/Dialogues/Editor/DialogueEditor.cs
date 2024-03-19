using UnityEditor;
using UnityEditor.Callbacks;

namespace RPG.Dialogues.Editor
{
    public class DialogueEditor : EditorWindow
    {
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowDialogueEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            DialogueSO dialogueSO = EditorUtility.InstanceIDToObject(instanceId) as DialogueSO;

            if (dialogueSO != null)
            {
                ShowDialogueEditorWindow();

                return true;
            }

            return false;
        }

    }
}
