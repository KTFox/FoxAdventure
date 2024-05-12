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
        public IEnumerator LoadLastSceneFromFile(string fileName)
        {
            Dictionary<string, object> state = GetSavedStateFromFile(fileName);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;

            if (state.ContainsKey("LastSceneIndex"))
            {
                buildIndex = (int)state["LastSceneIndex"];
            }

            yield return SceneManager.LoadSceneAsync(buildIndex);

            RestoreSaveableEntitiesBy(state);
        }

        public void SaveGameStateIntoFile(string fileName)
        {
            Dictionary<string, object> state = GetSavedStateFromFile(fileName);

            CaptureSaveableEntitiesInto(state);
            SaveStateIntoFile(fileName, state);
        }

        public void RestoreGameStateFromFile(string fileName)
        {
            RestoreSaveableEntitiesBy(GetSavedStateFromFile(fileName));
        }

        public void DeleteSavedFile(string fileName)
        {
            File.Delete(GetPathOf(fileName));
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

        private void SaveStateIntoFile(string saveFile, Dictionary<string, object> state)
        {
            string path = GetPathOf(saveFile);

            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }

            Debug.Log($"Saving to {path}");
        }

        private Dictionary<string, object> GetSavedStateFromFile(string fileName)
        {
            string path = GetPathOf(fileName);

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

        private void CaptureSaveableEntitiesInto(Dictionary<string, object> state)
        {
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity saveableEntity in saveableEntities)
            {
                state[saveableEntity.UniqueIdentifier] = saveableEntity.CaptureAllISaveableComponents();
            }

            state["LastSceneIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreSaveableEntitiesBy(Dictionary<string, object> state)
        {
            SaveableEntity[] saveableEntities = FindObjectsOfType<SaveableEntity>();

            foreach (SaveableEntity saveableEntity in saveableEntities)
            {
                string identifier = saveableEntity.UniqueIdentifier;

                if (state.ContainsKey(identifier))
                {
                    saveableEntity.RestoreISaveableComponents(state[identifier]);
                }
            }
        }

        private string GetPathOf(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".sav");
        }
    }
}
