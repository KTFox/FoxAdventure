using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace RPG.Saving {
    public class SavingSystem : MonoBehaviour {

        public void Save(string saveFile) {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");

            using (FileStream stream = File.Open(path, FileMode.Create)) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, CaptureState());
            }
        }

        public void Load(string saveFile) {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Loading from {path}");

            using (FileStream stream = File.Open(path, FileMode.Open)) {
                BinaryFormatter formatter = new BinaryFormatter();
                RestoreState(formatter.Deserialize(stream));
            }
        }

        private object CaptureState() {
            Dictionary<string, object> state = new Dictionary<string, object>();
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity saveableEntity in saveableEntities) {
                state[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureState();
            }

            return state;
        }

        private void RestoreState(object state) {
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();
            Dictionary<string, object> stateDict = (Dictionary<string, object>)state;

            foreach (SaveableEntity saveableEntity in saveableEntities) {
                saveableEntity.RestoreState(stateDict[saveableEntity.GetUniqueIdentifier()]);
            }
        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
