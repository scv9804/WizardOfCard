using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;

using System;
using System.IO;

namespace BETA
{
    // ==================================================================================================== JSON

    public struct JSON<T>
    {
        // ==================================================================================================== Field

        // =========================================================================== Data

        [SerializeField]
        private string _name;

        [SerializeField] [TextArea(1, 128)]
        private string _data;

        // =========================================================================== JSON

        private static JsonSerializerSettings s_settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,

            TypeNameHandling = TypeNameHandling.Auto
        };

        // ==================================================================================================== Property

        // =========================================================================== Data

        public string Name
        {
            get
            {
                return _name;
            }

            private set
            {
                _name = value;
            }
        }

        public string Data
        {
            get
            {
                return _data;
            }

            private set
            {
                _data = value;
            }
        }

        public T Source
        {
            get
            {
                return Deserialize(Data);
            }

            set
            {
                Data = Serialize(Name, value);
            }
        }

        // ==================================================================================================== Method

        // =========================================================================== Constructor

        public JSON(string name)
        {
            _name = name;
            _data = Read(name);
        }

        public JSON(string name, T source)
        {
            _name = name;
            _data = Serialize(name, source);
        }

        // =========================================================================== Operator

        public static implicit operator string(JSON<T> json)
        {
            return json.Data;
        }

        // =========================================================================== JSON

        private static string Serialize(string name, T model)
        {
            var data = JsonConvert.SerializeObject(model, s_settings);

            Write(name, data);

            return data;
        }

        private static T Deserialize(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, s_settings);
        }

        // =========================================================================== IO

        private static FileInfo GetFile(string name)
        {
            var root = Application.isEditor ? Application.dataPath : Application.persistentDataPath;
            var path = $"{root}/BETA/Resources/Save/{name}.json";

            return new FileInfo(path);
        }

        private static void Write(string name, string data)
        {
            var file = GetFile(name);
            file.Directory.Create();

            File.WriteAllText(file.FullName, data);
        }

        private static string Read(string name)
        {
            var file = GetFile(name);

            return File.ReadAllText(file.FullName);
        }
    }
}