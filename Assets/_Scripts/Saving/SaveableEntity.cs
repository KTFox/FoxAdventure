using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private string _uniqueIdentifier = "";

        private static Dictionary<string, SaveableEntity> globalLookup = new Dictionary<string, SaveableEntity>();

        // Properties

        public string UniqueIdentifier=> _uniqueIdentifier;


        // Methods

        public Dictionary<string, object> CaptureAllISaveableComponents()
        {
            var results = new Dictionary<string, object>();

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                results[saveable.GetType().ToString()] = saveable.CaptureState();
            }

            return results;
        }

        public void RestoreAllISavableComponents(object state)
        {
            var stateDict = (Dictionary<string, object>)state;

            foreach (var saveable in GetComponents<ISaveable>())
            {
                string typeString = saveable.GetType().ToString();

                if (stateDict.ContainsKey(typeString))
                {
                    saveable.RestoreState(stateDict[typeString]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            // Path will be null or empty when gameObject is in prefab folder 
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; 

            // In order to change the values that are being stored into the scene file or prefab, use SerializedObject and SerializedProperty 
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_uniqueIdentifier");

            // Auto generate unique _portalIdentifier 
            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;

            Debug.Log("Editting");
        }

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate))
            {
                return true;
            }

            if (globalLookup[candidate] == this)
            {
                return true;
            }

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);

                return true;
            }

            if (globalLookup[candidate]._uniqueIdentifier != candidate)
            {
                globalLookup.Remove(candidate);

                return true;
            }

            return false;
        }
#endif
    }
}
