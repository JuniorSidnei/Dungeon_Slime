using System.Collections;
using System.Collections.Generic;
using System.IO;
using DungeonSlime.Utils;
using GameToBeNamed.Utils;
using UnityEngine;

namespace DungeonSlime.Managers {

    public static class SaveManager {
        
        private static string m_directory = "/UserData/";
        private static string m_fileName = "userData.txt";
        
        public static void SaveData(UserData userData) {

            var dir = Application.persistentDataPath + m_directory;

            if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
            }

            
            var json = JsonUtility.ToJson(userData);
            File.WriteAllText(dir + m_fileName, json);
        }

        public static UserData LoadData() {

            var filePath = Application.persistentDataPath + m_directory + m_fileName;
            var userData = new UserData();
            
            if (File.Exists(filePath)) {
                var jsonData = File.ReadAllText(filePath);
                userData = JsonUtility.FromJson<UserData>(jsonData);
            }
            else {
                Debug.Log("Save file doesn't exist!");
            }

            return userData;
        }
    }
}