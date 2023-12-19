using UnityEngine;

using Newtonsoft.Json;

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Reworked
{
    // ==================================================================================================== Game

    public static partial class Game
    {
        // ==================================================================================================== Fixed

        // =========================================================================== Identifier

        private const string INSTANCE_ID_FORMAT = "D6";

        // ==================================================================================================== Field

        // =========================================================================== StringBuilder

        private static StringBuilder s_stringBuilder = new StringBuilder();

        // =========================================================================== Data

        // ================================================== Data

        private static Data s_data = new Data();

        // ================================================== Rijndael

        private static RijndaelManaged s_cipher = new RijndaelManaged()
        {
            Mode = CipherMode.CBC,
            Padding = PaddingMode.PKCS7,

            KeySize = 128,
            BlockSize = 128,

            Key = new byte[32]
            {
                61,
                107,
                176,
                86,
                71,
                87,
                114,
                73,
                38,
                53,
                174,
                246,
                102,
                212,
                175,
                13,
                204,
                78,
                53,
                86,
                77,
                179,
                224,
                52,
                78,
                97,
                180,
                58,
                247,
                102,
                248,
                4
            },

            IV = new byte[16]
            {
                223,
                28,
                160,
                248,
                222,
                213,
                53,
                145,
                9,
                106,
                56,
                105,
                9,
                98,
                148,
                23
            }
        };

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

        // =========================================================================== Identifier

        public static string Allocate(InstanceType type)
        {
            s_stringBuilder.Clear();

            switch (type)
            {
                case InstanceType.BOSS:
                    s_stringBuilder.Append("B");
                    break;
                case InstanceType.CARD:
                    s_stringBuilder.Append("C");
                    break;
                case InstanceType.ENEMY:
                    s_stringBuilder.Append("E");
                    break;
                case InstanceType.ITEM:
                    s_stringBuilder.Append("I");
                    break;
                case InstanceType.PLAYER:
                    s_stringBuilder.Append("P");
                    break;
            }

            var alloceted = s_data.Allocated.ToString(INSTANCE_ID_FORMAT);

            s_stringBuilder.Append(alloceted);

            s_data.Allocated += 1;

            return s_stringBuilder.ToString();
        }

        // =========================================================================== Data

        // ================================================== Data

        public static void Clear()
        {
            s_data.Clear();
        }

        // ================================================== JSON

        public static void Save<T>(T data, string name)
        {
            try
            {
                string saveData = JsonConvert.SerializeObject(data, Formatting.Indented);

                saveData = Encrypt(saveData);

                Write(saveData, name);
            }
            catch (Exception e)
            {
                #region #if UNITY_EDITOR => Debug.LogError();
#if UNITY_EDITOR
                Debug.LogError($"! SAVE ERROR ! {e}");
#endif
                #endregion
            }
        }

        public static T Load<T>(string name)
        {
            try
            {
                string saveData = Read(name);

                saveData = Decrypt(saveData);

                return JsonConvert.DeserializeObject<T>(saveData);

            }
            catch (Exception e)
            {
                #region #if UNITY_EDITOR => Debug.LogError();
#if UNITY_EDITOR
                Debug.LogError($"! LOAD ERROR ! {e}");
#endif
                #endregion

                return default;
            }
        }

        // ================================================== Rijndael

        private static string Encrypt(string data)
        {
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                using (var transform = s_cipher.CreateEncryptor())
                {
                    bytes = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                }

                return Convert.ToBase64String(bytes);

            }
            catch (Exception e)
            {
                #region #if UNITY_EDITOR => Debug.LogError();
#if UNITY_EDITOR
                Debug.LogError($"! ENCRYPT ERROR ! {e}");
#endif
                #endregion

                return data;
            }
        }

        private static string Decrypt(string data)
        {
            try
            {
                byte[] bytes = Convert.FromBase64String(data);

                using (var transform = s_cipher.CreateDecryptor())
                {
                    bytes = transform.TransformFinalBlock(bytes, 0, bytes.Length);
                }

                return Encoding.UTF8.GetString(bytes);

            }
            catch (Exception e)
            {
                #region #if UNITY_EDITOR => Debug.LogError();
#if UNITY_EDITOR
                Debug.LogError($"! DECRYPT ERROR ! {e}");
#endif
                #endregion

                return data;
            }
        }

        // ================================================== IO

        private static void Write(string data, string name)
        {
            try
            {
                string path = GetDataPath("Resources/Save", name);

                var file = new FileInfo(path);
                file.Directory.Create();

                File.WriteAllText(file.FullName, data);
            }
            catch (Exception e)
            {
                #region #if UNITY_EDITOR => Debug.LogError();
#if UNITY_EDITOR
                Debug.LogError($"! WRITE ERROR ! {e}");
#endif
                #endregion
            }
        }

        private static string Read(string name)
        {
            try
            {
                string path = GetDataPath("Resources/Save", name);

                var file = new FileInfo(path);

                return File.ReadAllText(file.FullName);
            }
            catch (Exception e)
            {
                #region #if UNITY_EDITOR => Debug.LogError();
#if UNITY_EDITOR
                Debug.LogError($"! READ ERROR ! {e}");
#endif
                #endregion

                return string.Empty;
            }
        }

        private static string GetDataPath(string directory, string name)
        {
            return $"{s_root}/{directory}/{name}";
        }
    }

    // ==================================================================================================== InstanceType

    public enum InstanceType
    {
        BOSS,

        CARD,

        ENEMY,

        ITEM,

        PLAYER
    }
}
