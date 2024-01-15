using System;
using System.IO;
using UnityEngine;

namespace RPG.Saving {
    public class SavingSystem : MonoBehaviour {

        public void Save(string saveFile) {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Saving to {path}");

            using (FileStream stream = File.Open(path, FileMode.Create)) {
                Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                byte[] buffer = SerializeVector3(playerTransform.position);

                stream.Write(SerializeVector3(playerTransform.position), 0, buffer.Length);
            }
        }

        public void Load(string saveFile) {
            string path = GetPathFromSaveFile(saveFile);
            Debug.Log($"Loading from {path}");

            using (FileStream stream = File.Open(path, FileMode.Open)) {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);

                Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
                playerTransform.position = DeSerializeVector3(buffer);
            }
        }

        private byte[] SerializeVector3(Vector3 vector) {
            byte[] vector3Bytes = new byte[3 * 4]; //Need 4 bytes to store a float numeber

            BitConverter.GetBytes(vector.x).CopyTo(vector3Bytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vector3Bytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vector3Bytes, 8);

            return vector3Bytes;
        }

        private Vector3 DeSerializeVector3(byte[] buffer) {
            Vector3 vector3;

            vector3.x = BitConverter.ToSingle(buffer, 0);
            vector3.y = BitConverter.ToSingle(buffer, 4);
            vector3.z = BitConverter.ToSingle(buffer, 8);

            return vector3;
        }

        private string GetPathFromSaveFile(string saveFile) {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
