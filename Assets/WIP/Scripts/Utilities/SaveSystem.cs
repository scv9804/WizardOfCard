using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace WIP
{
    public static class SaveSystem
    {
        // ==================================================================================================== Fields

        // =========================================================================== File Path

        public static readonly string[] DefaultPath = new string[2]
        {
            "WIP", "Save"
        };

        // =========================================================================== StringBuilder

        private static StringBuilder s_stringBuilder = new StringBuilder();

        // ==================================================================================================== Properties

        // =========================================================================== File Path

        private static string FilePath
        {
            get
            {
                return Application.isEditor ? Application.dataPath : Application.persistentDataPath;
            }
        }

        // ==================================================================================================== Methods

        // =========================================================================== Json

        public static void Save<TData>(TData data, string name, params string[] directory)
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
                string jsonPath = GetFilePath(name, directory);

                Write(jsonData, jsonPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"! SAVE ERROR ! {e}");
            }
        }

        public static TData Load<TData>(string name, params string[] directory)
        {
            try
            {
                string jsonData;
                string jsonPath = GetFilePath(name, directory);

                jsonData = Read(jsonPath);

                return JsonConvert.DeserializeObject<TData>(jsonData);
            }
            catch (Exception e)
            {
                Debug.LogError($"! LOAD ERROR ! {e}");

                return default;
            }
        }

        public static void Delete(string name, params string[] directory)
        {
            try
            {
                string path = GetFilePath(name, directory);

                File.Delete(path);
                if (Application.isEditor)
                {
                    File.Delete($"{path}.meta");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"! DELETE ERROR ! {e}");
            }
        }

        // =========================================================================== Text Asset

        private static void Write(string data, string path)
        {
            try
            {
                var file = new FileInfo(path);
                file.Directory.Create();

                File.WriteAllText(file.FullName, data);
            }
            catch (Exception e)
            {
                Debug.LogError($"! WRITE ERROR ! {e}");
            }
        }

        private static string Read(string path)
        {
            try
            {
                var file = new FileInfo(path);

                return File.ReadAllText(file.FullName);
            }
            catch (Exception e)
            {
                Debug.LogError($"! READ ERROR ! {e}");

                return default;
            }
        }

        // =========================================================================== File Path

        private static string GetFilePath(string name, params string[] directory)
        {
            s_stringBuilder.Clear();
            s_stringBuilder.Append(FilePath);

            for (int i = 0; i < directory.Length; i++)
            {
                s_stringBuilder.Append($"/{directory[i]}");
            }

            s_stringBuilder.Append($"/{name}.json");

            string path = s_stringBuilder.ToString();

            Debug.Log(path);

            return path;
        }
    }
}
