using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogues.Editor
{
    public class DialogueEditor : EditorWindow
    {
        private DialogueSO selectedDialogue;
        private GUIStyle dialogueNodeStyle;


        [MenuItem("Window/Dialogue Editor")]
        public static void ShowDialogueEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
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

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            dialogueNodeStyle = new GUIStyle();
            dialogueNodeStyle.normal.background = EditorGUIUtility.Load("Node0") as Texture2D;
            dialogueNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            dialogueNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        void OnSelectionChanged()
        {
            DialogueSO newDialogue = Selection.activeObject as DialogueSO;

            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue != null)
            {
                foreach (DialogueNode dialogueNode in selectedDialogue.DialogueNodes)
                {
                    OnGUINode(dialogueNode);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No dialogue selected.");
            }
        }

        private void OnGUINode(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.Position, dialogueNodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Nodes: ");
            string newText = EditorGUILayout.TextField(dialogueNode.DialogueText);
            string newUniqueID = EditorGUILayout.TextField(dialogueNode.UniqueId);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update DialogueSO");

                dialogueNode.DialogueText = newText;
                dialogueNode.UniqueId = newUniqueID;
            }

            GUILayout.EndArea();
        }
    }
}
