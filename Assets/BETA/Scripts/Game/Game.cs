using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BETA
{
    // ==================================================================================================== Game

    public static partial class Game
    {
        // ==================================================================================================== Property

        // =========================================================================== IO

        private static string s_root
        {
            get
            {
                return Application.isEditor ? Application.dataPath : Application.persistentDataPath;
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Data

        // ================================================== JSON

        public static void Save<T>(T data, string directory, string name)
        {
            try
            {
                string saveData = JsonConvert.SerializeObject(data, Formatting.Indented);

                saveData = Encrypt(saveData);

                Write(saveData, directory, name);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! SAVE ERROR ! {e}");
            }
        }

        public static T Load<T>(string directory, string name)
        {
            try
            {
                string saveData = Read(directory, name);

                saveData = Decrypt(saveData);

                return JsonConvert.DeserializeObject<T>(saveData);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! LOAD ERROR ! {e}");

                return default;
            }
        }

        // ================================================== BETA

        public static void SaveWithoutRijndael<T>(T data, string directory, string name)
        {
            try
            {
                string saveData = JsonConvert.SerializeObject(data, Formatting.Indented);

                Write(saveData, directory, name);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! SAVE ERROR ! {e}");
            }
        }

        public static T LoadWithoutRijndael<T>(string directory, string name)
        {
            try
            {
                string saveData = Read(directory, name);

                return JsonConvert.DeserializeObject<T>(saveData);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! LOAD ERROR ! {e}");

                return default;
            }
        }

        // ================================================== Rijndael

        private static string Encrypt(string data)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                using (var cipher = new RijndaelManaged())
                {
                    cipher.Mode = CipherMode.CBC;
                    cipher.Padding = PaddingMode.PKCS7;

                    cipher.KeySize = 128;
                    cipher.BlockSize = 128;

                    cipher.Key = Convert.FromBase64String("PWuwVkdXckkmNa72ZtSvDcxONVZNs+A0");
                    cipher.IV = Convert.FromBase64String("3xyg+N7VNZEJajhpCWKUFw==");

                    using (var transform = cipher.CreateEncryptor())
                    {
                        bytes = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                    }
                }

                return Convert.ToBase64String(bytes);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! ENCRYPT ERROR ! {e}");

                return data;
            }
        }

        private static string Decrypt(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);

                using (var cipher = new RijndaelManaged())
                {
                    cipher.Mode = CipherMode.CBC;
                    cipher.Padding = PaddingMode.PKCS7;

                    cipher.KeySize = 128;
                    cipher.BlockSize = 128;

                    cipher.Key = Convert.FromBase64String("PWuwVkdXckkmNa72ZtSvDcxONVZNs+A0");
                    cipher.IV = Convert.FromBase64String("3xyg+N7VNZEJajhpCWKUFw==");

                    using (var transform = cipher.CreateDecryptor())
                    {
                        bytes = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                    }
                }

                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! DECRYPT ERROR ! {e}");

                return data;
            }
        }

        // ================================================== IO

        private static void Write(string data, string directory, string name)
        {
            try
            {
                string path = GetDataPath(directory, name);

                var file = new FileInfo(path);
                file.Directory.Create();

                File.WriteAllText(file.FullName, data);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! WRITE ERROR ! {e}");
            }
        }

        private static string Read(string directory, string name)
        {
            try
            {
                string path = GetDataPath(directory, name);

                var file = new FileInfo(path);

                return File.ReadAllText(file.FullName);
            }
            catch (Exception e)
            {
                EditorDebug.EditorLogError($"! READ ERROR ! {e}");

                return string.Empty;
            }
        }

        private static string GetDataPath(string directory, string name)
        {
            return $"{s_root}/{directory}/{name}.json";
        }
    }
}