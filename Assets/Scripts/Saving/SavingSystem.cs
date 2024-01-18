using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving {
    public class SavingSystem : MonoBehaviour {

        public IEnumerator LoadLastScene(string saveFile) {
            Dictionary<string, object> state = LoadFile(saveFile);

            if (state.ContainsKey("LastSceneIndex")) {
                int buildIndex = (int)state["LastSceneIndex"];

                if (buildIndex != SceneManager.GetActiveScene().buildIndex) {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }

            RestoreSaveableEntityState(state);
        }

        /// <summary>
        /// Save all SaveableEntity states into saveFile
        /// </summary>
        /// <param name="saveFile"></param>
        public void Save(string saveFile) {
            Dictionary<string, object> state = LoadFile(saveFile);

            CaptureSaveableEntityState(state);
            SaveFile(saveFile, state);
        }

        /// <summary>
        /// Load all SaveableEntity states from saveFile
        /// </summary>
        /// <param name="saveFile"></param>
        public void Load(string saveFile) {
            RestoreSaveableEntityState(LoadFile(saveFile));
        }

        public void Delete(string saveFile) {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        private void SaveFile(string saveFile, Dictionary<string, object> state) {
            string path = GetPathFromSaveFile(saveFile);

            using (FileStream stream = File.Open(path, FileMode.Create)) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }

            Debug.Log($"Saving to {path}");
        }

        /// <summary>
        /// Return an empty dictionary if saveFile doesn't exist
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns></returns>
        private Dictionary<string, object> LoadFile(string saveFile) {
            string path = GetPathFromSaveFile(saveFile);

            if (!File.Exists(path)) {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(path, FileMode.Open)) {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void CaptureSaveableEntityState(Dictionary<string, object> state) {
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity saveableEntity in saveableEntities) {
                state[saveableEntity.GetUniqueIdentifier()] = saveableEntity.CaptureISaveableState();
            }

            state["LastSceneIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreSaveableEntityState(Dictionary<string, object> state) {
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity saveableEntity in saveableEntities) {
                string identifier = saveableEntity.GetUniqueIdentifier();

                if (state.ContainsKey(identifier)) {
                    saveableEntity.RestoreISaveableState(state[identifier]);
                }
            }
        }

        /// <summary>
        /// Get Persistent Data Path from saveFile
        /// </summary>
        /// <param name="saveFile"></param>
        /// <returns></returns>
        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
