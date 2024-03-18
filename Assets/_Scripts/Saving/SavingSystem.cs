using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFileFromPath(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            if (state.ContainsKey("LastSceneIndex"))
            {
                buildIndex = (int)state["LastSceneIndex"];
            }

            yield return SceneManager.LoadSceneAsync(buildIndex);

            RestoreAllSaveableEntities(state);
        }

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFileFromPath(saveFile);

            CaptureAllSaveableEntities(state);
            SaveFileToPath(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreAllSaveableEntities(LoadFileFromPath(saveFile));
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetPathFromSaveFile(saveFile));
        }

        public IEnumerable<string> GetSaveFileNames()
        {
            foreach (string path in Directory.EnumerateFiles(Application.persistentDataPath))
            {
                if (Path.GetExtension(path) == ".sav")
                {
                    yield return Path.GetFileNameWithoutExtension(path);
                }
            }
        }

        private void SaveFileToPath(string saveFile, Dictionary<string, object> state)
        {
            string path = GetPathFromSaveFile(saveFile);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }

            Debug.Log($"Saving to {path}");
        }

        private Dictionary<string, object> LoadFileFromPath(string saveFile)
        {
            string path = GetPathFromSaveFile(saveFile);

            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();

                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void CaptureAllSaveableEntities(Dictionary<string, object> state)
        {
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity saveableEntity in saveableEntities)
            {
                state[saveableEntity.UniqueIdentifier] = saveableEntity.CaptureAllISaveableComponents();
            }

            state["LastSceneIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreAllSaveableEntities(Dictionary<string, object> state)
        {
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity saveableEntity in saveableEntities)
            {
                string identifier = saveableEntity.UniqueIdentifier;

                if (state.ContainsKey(identifier))
                {
                    saveableEntity.RestoreAllISavableComponents(state[identifier]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
