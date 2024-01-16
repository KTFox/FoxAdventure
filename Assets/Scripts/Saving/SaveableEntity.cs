using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving {
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour {

        [SerializeField]
        private string uniqueIdentifier = "";

        public Dictionary<string, object> CaptureISaveableState() {
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>()) {
                result[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return result;
        }

        public void RestoreISaveableState(object state) {
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            foreach (ISaveable saveable in GetComponents<ISaveable>()) {
                string typeString = saveable.GetType().ToString();

                if (stateDict.ContainsKey(typeString)) {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

        public string GetUniqueIdentifier() {
            return uniqueIdentifier;
        }

        /// <summary>
        /// Auto generate unique identifier in Edit Mode.
        /// This Update will be not included in Build Project
        /// </summary>
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; //path will be null or empty when gameObject is in prefab folder 

            //In order to change the values that are being stored into the scene file or prefab, use SerializedObject and SerializedProperty 
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            //Generate unique identifier 
            if (string.IsNullOrEmpty(property.stringValue)) {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            Debug.Log("Editting");
        }
    }
}
