using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//************************************************************************************************************************************************************************
//Ejemplo para utilizar los metodos Save y Load
//public Vector3 posPlayer;
//SaveManager.saveManager.Save("pos", expPlayer.x, expPlayer.y, expPlayer.z); (Tenemos que agregar un key del tipo string al igual que los playerpref, en este caso "pos").
//posPlayer = SaveManager.saveManager.LoadVector3("pos");  (Para usar el Load llamamos de esa manera y simplemente ponemos la Key).
//************************************************************************************************************************************************************************

namespace com.Pizia.Saver
{
    public static class SaveManager
    {
        public static bool instantSave;

        public static string CurrentTag { get; private set; }

        static DataSave _SaveData;
        public static DataSave SaveData
        { 
            get
            {
                if (_SaveData == null) _SaveData = Saver.Load();
                return _SaveData;
            }

            private set => _SaveData = value;
        }
        
        public static void SaveAll() => Saver.Save(SaveData);

        public static void Clear() => SaveData = new DataSave();

        #region Save Sobrecargas

        public static void Save(string key, byte byteDato, string tag = "") => SaveByte(key, byteDato, tag);
        public static void Save(string key, short shortDato, string tag = "") => SaveShort(key, shortDato, tag);
        public static void Save(string key, int intDato, string tag = "") => SaveInt(key, intDato, tag);
        public static void Save(string key, long longDato, string tag = "") => SaveLong(key, longDato, tag);
        public static void Save(string key, float floatDato, string tag = "") => SaveFloat(key, floatDato, tag);
        public static void Save(string key, string stringDato, string tag = "") => SaveString(key, stringDato, tag);
        public static void Save(string key, bool boolDato, string tag = "") => SaveBool(key, boolDato, tag);
        public static void Save(string key, Vector2 vectorDato, string tag = "") => SaveVector2(key, vectorDato, tag);
        public static void Save(string key, Vector3 vectorDato, string tag = "") => SaveVector3(key, vectorDato, tag);
        public static void Save(string key, float x, float y, float z, string tag = "") => SaveVector3(key, x, y, z, tag);
        public static void Save(string key, BitFlag8 bitFlag, string tag = "") => SaveBitFlag(key, bitFlag, tag);
        public static void Save<T>(string key, IList array, string tag = "") => SaveCollection<T>(key, array, tag);
        public static void Save<T>(string key, IDictionary dictionary, string tag = "") => SaveDictionary<T>(key, dictionary, tag);

        #region SaveTypeSpecific

        public static void SaveByte(string key, byte byteDato, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.byteSave.ContainsKey(key))
                SaveData.byteSave.Add(key, byteDato);
            else
                SaveData.byteSave[key] = byteDato;

            //ShowInt.Add(new ShowInt(key, byteDato));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveShort(string key, short shortDato, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.shortSave.ContainsKey(key))
                SaveData.shortSave.Add(key, shortDato);
            else
                SaveData.shortSave[key] = shortDato;

            //ShowInt.Add(new ShowInt(key, shortDato));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveInt(string key, int intDato, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.intSave.ContainsKey(key))
                SaveData.intSave.Add(key, intDato);
            else
                SaveData.intSave[key] = intDato;

            //ShowInt.Add(new ShowInt(key, intDato));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveLong(string key, long longDato, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.longSave.ContainsKey(key))
                SaveData.longSave.Add(key, longDato);
            else
                SaveData.longSave[key] = longDato;

            //ShowLong.Add(new ShowLong(key, longDato));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveFloat(string key, float floatDato, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.floatSave.ContainsKey(key))
                SaveData.floatSave.Add(key, floatDato);
            else
                SaveData.floatSave[key] = floatDato;

            //ShowFloat.Add(new ShowFloat(key, floatDato));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveString(string key, string stringDato, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.stringSave.ContainsKey(key))
                SaveData.stringSave.Add(key, stringDato);
            else
                SaveData.stringSave[key] = stringDato;

            //ShowString.Add(new ShowString(key, stringDato));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveBool(string key, bool boolDato, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.boolSave.ContainsKey(key))
                SaveData.boolSave.Add(key, boolDato);
            else
                SaveData.boolSave[key] = boolDato;

            //ShowBool.Add(new ShowBool(key, boolDato));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveVector2(string key, Vector2 vector, string tag = "") => SaveVector3(key, vector.x, vector.y, 0, tag);
        public static void SaveVector3(string key, Vector3 vector, string tag = "") => SaveVector3(key, vector.x, vector.y, vector.z, tag);

        public static void SaveVector3(string key, float x, float y, float z, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.vector3Save.ContainsKey(key))
                SaveData.vector3Save.Add(key, new Vector3(x, y, z));
            else
                SaveData.vector3Save[key] = new Vector3(x, y, z);

            //ShowInInspector();
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveBitFlag(string key, BitFlag8 bitFlag, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.byteSave.ContainsKey(key))
                SaveData.byteSave.Add(key, bitFlag.GetByte());
            else
                SaveData.byteSave[key] = bitFlag.GetByte();

            //ShowInt.Add(new ShowInt(key, bitFlag.GetByte()));
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveCollection<T>(string key, IList array, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.arraySave.ContainsKey(key))
                SaveData.arraySave.Add(key, new ArrayWrapper(typeof(T), array));
            else
                SaveData.arraySave[key] = new ArrayWrapper(typeof(T), array);

            //ShowArray.Add(key);
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        public static void SaveDictionary<T>(string key, IDictionary dictionary, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (!SaveData.dictionarySave.ContainsKey(key))
                SaveData.dictionarySave.Add(key, new DictionaryWrapper(typeof(T), dictionary.Keys, dictionary.Values));
            else
                SaveData.dictionarySave[key] = new DictionaryWrapper(typeof(T), dictionary.Keys, dictionary.Values);

            //ShowArray.Add(key);
            if (instantSave) SaveAll();
            //ShowInInspector();
        }

        #endregion
        
        #endregion

        public static void ResetAllByTag(string tag) => SaveData.ResetByTag(tag);

        public static void DeleteKey(string key, string tag = "") => SaveData.DeleteKey($"{key}_{(tag != "" ? tag : CurrentTag)}");
        public static void DeleteAllKeysByTag(string tag) => SaveData.DeleteAllKeysByTag(tag);
        public static void DeleteAllKeys() => SaveData.DeleteAllKeys();

        public static bool HasKey(string key, string tag = "") => SaveData.HasKey($"{key}_{(tag != "" ? tag : CurrentTag)}");
        public static bool HasTag(string tag) => SaveData.HasTag(tag);
        public static List<string> GetKeysByTag(string tag) => SaveData.GetKeysByTag(tag);
        public static void SetCurrentGlobalTag(string newTag) => CurrentTag = newTag;

        #region Load Sobrecargas

        public static void Load(string key, out int intDato, string tag = "") => intDato = LoadInt(key, tag);
        public static void Load(string key, out byte byteDato, string tag = "") => byteDato = LoadByte(key, tag);
        public static void Load(string key, out short shortDato, string tag = "") => shortDato = LoadShort(key, tag);
        public static void Load(string key, out long longDato, string tag = "") => longDato = LoadLong(key, tag);
        public static void Load(string key, out bool boolDato, string tag = "") => boolDato = LoadBool(key, tag);
        public static void Load(string key, out float floatDato, string tag = "") => floatDato = LoadFloat(key, tag);
        public static void Load(string key, out string stringDato, string tag = "") => stringDato = LoadString(key, tag);
        public static void Load(string key, out Vector2 vector, string tag = "") => vector = LoadVector2(key, tag);
        public static void Load(string key, out Vector3 vector, string tag = "") => vector = LoadVector3(key, tag);
        public static void Load(string key, out BitFlag8 bitFlag, string tag = "") => bitFlag = LoadBitFlag8(key, tag);
        public static void Load<T>(string key, out T[] array, string tag = "") => array = LoadArray<T>(key, tag);
        public static void Load<T>(string key, out List<T> list, string tag = "") => list = LoadList<T>(key, tag);
        public static void Load<T>(string key, out Dictionary<string, T> dictionary, string tag = "") => dictionary = LoadDictionary<T>(key, tag);

        #region LoadTypeSpecific
        
        public static int LoadInt(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.intSave.TryGetValue(key, out int value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return 0;
            }
        }

        public static byte LoadByte(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.byteSave.TryGetValue(key, out byte value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return 0;
            }
        }

        public static short LoadShort(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.shortSave.TryGetValue(key, out short value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return 0;
            }
        }

        public static long LoadLong(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.longSave.TryGetValue(key, out long value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return 0;
            }
        }

        public static float LoadFloat(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.floatSave.TryGetValue(key, out float value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return 0;
            }
        }

        public static string LoadString(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.stringSave.TryGetValue(key, out string value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return "";
            }
        }

        public static bool LoadBool(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.boolSave.TryGetValue(key, out bool value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return false;
            }
        }

        public static Vector2 LoadVector2(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.vector3Save.TryGetValue(key, out Vector3 value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return Vector2.zero;
            }
        }

        public static Vector3 LoadVector3(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.vector3Save.TryGetValue(key, out Vector3 value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return Vector3.zero;
            }
        }

        public static BitFlag8 LoadBitFlag8(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.byteSave.TryGetValue(key, out byte value))
            {
                return new BitFlag8(value);
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return new BitFlag8();
            }
        }

        public static T[] LoadArray<T>(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.arraySave.TryGetValue(key, out ArrayWrapper value))
            {
                int size = value.array.Count;
                T[] array = new T[size];

                for (int i = 0; i < size; i++) array[i] = (T) value.array[i];
                return array;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return null;
            }
        }

        public static List<T> LoadList<T>(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.arraySave.TryGetValue(key, out ArrayWrapper value))
            {
                List<T> array = new List<T>();
                int size = value.array.Count;

                for (int i = 0; i < size; i++) array.Add((T) value.array[i]);
                return array;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return null;
            }
        }

        public static Dictionary<string, T> LoadDictionary<T>(string key, string tag = "")
        {
            key = $"{key}_{(tag != "" ? tag : CurrentTag)}";
            if (SaveData.dictionarySave.TryGetValue(key, out DictionaryWrapper value))
            {
                Dictionary<string, T> dictionary = new Dictionary<string, T>();
                int size = value.keys.Count;

                for (int i = 0; i < size; i++) dictionary.Add(value.keys[i], (T) value.values[i]);
                return dictionary;
            }
            else
            {
                Debug.LogWarning("No existe Key: " + key);
                return null;
            }
        }

        #endregion

        #endregion
    }
}
