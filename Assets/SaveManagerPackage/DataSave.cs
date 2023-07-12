using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace com.Pizia.Saver
{
    [System.Serializable]
    public sealed class DataSave
    {
        const int MAXDATA = 8;

        public readonly Dictionary<string, bool> boolSave;
        public readonly Dictionary<string, byte> byteSave;
        public readonly Dictionary<string, short> shortSave;
        public readonly Dictionary<string, int> intSave;
        public readonly Dictionary<string, long> longSave;
        public readonly Dictionary<string, float> floatSave;
        public readonly Dictionary<string, string> stringSave;
        public readonly Dictionary<string, Vector3> vector3Save;
        public readonly Dictionary<string, ArrayWrapper> arraySave;
        public readonly Dictionary<string, DictionaryWrapper> dictionarySave;

        public DataSave()
        {
            boolSave = new Dictionary<string, bool>();
            byteSave = new Dictionary<string, byte>();
            shortSave = new Dictionary<string, short>();
            intSave = new Dictionary<string, int>();
            longSave = new Dictionary<string, long>();
            floatSave = new Dictionary<string, float>();
            stringSave = new Dictionary<string, string>();
            vector3Save = new Dictionary<string, Vector3>();
            arraySave = new Dictionary<string, ArrayWrapper>();
            dictionarySave = new Dictionary<string, DictionaryWrapper>();
        }

        public bool HasKey(string key)
        {
            for (int i = 0; i < MAXDATA; i++)
            {
                if (this[i, key]) 
                    return true;
            }

            return false;
        }

        public bool HasTag(string tag)
        {
            for (int i = 0; i < MAXDATA; i++)
            {
                if (CheckTag(i, $"_{tag}"))
                    return true;
            }
            return false;
        }
        
        public List<string> GetKeysByTag(string tagToSearch)
        {
            List<string> keysWithTag = new List<string>();
            string tag = $"_{tagToSearch}";

            for (int i = 0; i < MAXDATA; i++)
            {
                if (CheckTag(i, tag, out string key))
                    keysWithTag.Add(key.Replace(tag, ""));
            }
            
            return keysWithTag;
        }

        public void ResetByTag(string tag)
        {
            for (int i = 0; i < MAXDATA; i++)
            {
                IDictionary dict = this[i];
                foreach (string key in dict.Keys) 
                {
                    if (key.Contains($"_{tag}"))
                        dict[key] = default;
                }
            }
        }

        public void DeleteKey(string key)
        {
            for (int i = 0; i < MAXDATA; i++)
            {
                if (this[i, key])
                {
                    this[i].Remove(key);
                    return;
                }
            }
        }

        public void DeleteAllKeysByTag(string tag)
        {
            for (int i = 0; i < MAXDATA; i++)
            {
                IDictionary dict = this[i];
                foreach (string key in dict.Keys)
                {
                    if (key.Contains($"_{tag}"))
                        dict.Remove(key);
                }
            }
        }

        public void DeleteAllKeys()
        {
            for (int i = 0; i < MAXDATA; i++) 
                this[i].Clear();
        }

        public void Serialize(BinaryWriter writer)
        {
            //writer.Write(boolSave.Count);
            //foreach (string key in boolSave.Keys)
            //{
            //    writer.Write(key);
            //    writer.Write(boolSave[key]);
            //}
            SaveBools(writer);

            writer.Write(byteSave.Count);
            foreach (string key in byteSave.Keys)
            {
                writer.Write(key);
                writer.Write(byteSave[key]);
            }

            writer.Write(shortSave.Count);
            foreach (string key in shortSave.Keys)
            {
                writer.Write(key);
                writer.Write(shortSave[key]);
            }

            writer.Write(intSave.Count);
            foreach (string key in intSave.Keys)
            {
                writer.Write(key);
                writer.Write(intSave[key]);
            }

            writer.Write(longSave.Count);
            foreach (string key in longSave.Keys)
            {
                writer.Write(key);
                writer.Write(longSave[key]);
            }

            writer.Write(floatSave.Count);
            foreach (string key in floatSave.Keys)
            {
                writer.Write(key);
                writer.Write(floatSave[key]);
            }

            writer.Write(stringSave.Count);
            foreach (string key in stringSave.Keys)
            {
                writer.Write(key);
                writer.Write(stringSave[key] != null ? stringSave[key] : "");
            }

            writer.Write(vector3Save.Count);
            foreach (string key in vector3Save.Keys)
            {
                writer.Write(key);
                writer.Write(vector3Save[key].x);
                writer.Write(vector3Save[key].y);
                writer.Write(vector3Save[key].z);
            }

            SaveArray(writer);
            SaveDictionary(writer);
        }

        void SaveBools(BinaryWriter writer)
        {
            int size = boolSave.Count, index = 0;
            bool[] boolArray = new bool[size];

            writer.Write(size);
            foreach (string key in boolSave.Keys)
            {
                writer.Write(key);
                boolArray[index] = boolSave[key];
                index++;
            }

            if (size < 9)
            {
                BitFlag8 bitFlag = new BitFlag8(boolArray);
                writer.Write(bitFlag.GetByte());
            }
            else if (size < 17)
            {
                BitFlag16 bitFlag = new BitFlag16(boolArray);
                writer.Write(bitFlag.GetShort());
            }
            else if (size < 33)
            {
                BitFlag32 bitFlag = new BitFlag32(boolArray);
                writer.Write(bitFlag.GetInt());
            }
            else if (size < 65)
            {
                BitFlag64 bitFlag = new BitFlag64(boolArray);
                writer.Write(bitFlag.GetLong());
            }
            else
            {
                int bitFlagQuantity = Mathf.RoundToInt((float) size / 64f);
                for (int i = 0; i < bitFlagQuantity; i++)
                {
                    bool[] currentBlock = GetBoolBlock(i, ref boolArray);

                    BitFlag64 bitFlag = new BitFlag64(currentBlock);
                    writer.Write(bitFlag.GetLong());
                }
            }
        }

        bool[] GetBoolBlock(int currentIndex, ref bool[] boolArray)
        {
            bool[] currentBlock = new bool[64];

            int quantity = 64 * (currentIndex + 1);
            if (quantity > boolArray.Length) quantity = boolArray.Length;
            for (int j = 64 * currentIndex, n = 0; j < quantity; j++, n++) currentBlock[n] = boolArray[j];
            return currentBlock;
        }

        void SaveArray(BinaryWriter writer)
        {
            writer.Write(arraySave.Count);
            foreach (string key in arraySave.Keys)
            {
                writer.Write(key);

                int size = arraySave[key].array.Count;
                writer.Write(size);

                System.Type type = arraySave[key].type;
                writer.Write(type.ToString());

                for (int i = 0; i < size; i++)
                {
                    if (type == typeof(bool))
                    {
                        writer.Write((bool) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(char))
                    {
                        writer.Write((char) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(byte))
                    {
                        writer.Write((byte) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(short))
                    {
                        writer.Write((short) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(int))
                    {
                        writer.Write((int) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(long))
                    {
                        writer.Write((long) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(float))
                    {
                        writer.Write((float) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(double))
                    {
                        writer.Write((double) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(decimal))
                    {
                        writer.Write((decimal) arraySave[key].array[i]);
                        continue;
                    }

                    if (type == typeof(string))
                    {
                        writer.Write((string) arraySave[key].array[i]);
                        continue;
                    }
                }
            }
        }

        void SaveDictionary(BinaryWriter writer)
        {
            writer.Write(dictionarySave.Count);
            foreach(string key in dictionarySave.Keys)
            {
                writer.Write(key);
                int size = dictionarySave[key].keys.Count;
                writer.Write(size);

                System.Type type = dictionarySave[key].dictionaryType;
                writer.Write(type.ToString());

                for (int i = 0; i < size; i++)
                {
                    writer.Write(dictionarySave[key].keys[i]);

                    if (type == typeof(bool))
                    {
                        writer.Write((bool) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(char))
                    {
                        writer.Write((char) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(byte))
                    {
                        writer.Write((byte) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(short))
                    {
                        writer.Write((short) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(int))
                    {
                        writer.Write((int) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(long))
                    {
                        writer.Write((long) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(float))
                    {
                        writer.Write((float) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(double))
                    {
                        writer.Write((double) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(decimal))
                    {
                        writer.Write((decimal) dictionarySave[key].values[i]);
                        continue;
                    }

                    if (type == typeof(string))
                    {
                        writer.Write((string) dictionarySave[key].values[i]);
                        continue;
                    }
                }
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            //int size = reader.ReadInt32();
            //for (int i = 0; i < size; i++)
            //{
            //    string key = reader.ReadString();
            //    bool value = reader.ReadBoolean();
            //    boolSave.Add(key, value);
            //}

            LoadBools(reader);

            int size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                byte value = reader.ReadByte();
                byteSave.Add(key, value);
            }

            size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                short value = reader.ReadInt16();
                shortSave.Add(key, value);
            }

            size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                int value = reader.ReadInt32();
                intSave.Add(key, value);
            }

            size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                long value = reader.ReadInt64();
                longSave.Add(key, value);
            }

            size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                float value = reader.ReadSingle();
                floatSave.Add(key, value);
            }

            size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                string value = reader.ReadString();
                stringSave.Add(key, value);
            }

            size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                Vector3 value = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                vector3Save.Add(key, value);
            }

            LoadArray(reader);
            LoadDictionary(reader);
        }

        void LoadArray(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                int arraySize = reader.ReadInt32();
                System.Type arrayType = System.Type.GetType(reader.ReadString());

                if (arrayType == typeof(bool))
                {
                    List<bool> array = new List<bool>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadBoolean());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(char))
                {
                    List<char> array = new List<char>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadChar());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(byte))
                {
                    List<byte> array = new List<byte>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadByte());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(short))
                {
                    List<short> array = new List<short>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadInt16());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(int))
                {
                    List<int> array = new List<int>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadInt32());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(long))
                {
                    List<long> array = new List<long>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadInt64());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(float))
                {
                    List<float> array = new List<float>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadSingle());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(double))
                {
                    List<double> array = new List<double>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadDouble());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(decimal))
                {
                    List<decimal> array = new List<decimal>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadDecimal());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }

                if (arrayType == typeof(string))
                {
                    List<string> array = new List<string>();
                    for (int j = 0; j < arraySize; j++) array.Add(reader.ReadString());
                    arraySave.Add(key, new ArrayWrapper(arrayType, array));
                    continue;
                }
            }
        }

        void LoadDictionary(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                string key = reader.ReadString();
                int dictionarySize = reader.ReadInt32();
                System.Type dicType = System.Type.GetType(reader.ReadString());

                if (dicType == typeof(bool))
                {
                    Dictionary<string, bool> loadedDictionary = new Dictionary<string, bool>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadBoolean());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }
                
                if (dicType == typeof(char))
                {
                    Dictionary<string, char> loadedDictionary = new Dictionary<string, char>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadChar());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(byte))
                {
                    Dictionary<string, byte> loadedDictionary = new Dictionary<string, byte>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadByte());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(short))
                {
                    Dictionary<string, short> loadedDictionary = new Dictionary<string, short>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadInt16());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(int))
                {
                    Dictionary<string, int> loadedDictionary = new Dictionary<string, int>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadInt32());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(long))
                {
                    Dictionary<string, long> loadedDictionary = new Dictionary<string, long>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadInt64());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(float))
                {
                    Dictionary<string, float> loadedDictionary = new Dictionary<string, float>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadSingle());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(double))
                {
                    Dictionary<string, double> loadedDictionary = new Dictionary<string, double>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadDouble());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(decimal))
                {
                    Dictionary<string, decimal> loadedDictionary = new Dictionary<string, decimal>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadDecimal());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }

                if (dicType == typeof(string))
                {
                    Dictionary<string, string> loadedDictionary = new Dictionary<string, string>();
                    for (int j = 0; j < dictionarySize; j++) loadedDictionary.Add(reader.ReadString(), reader.ReadString());
                    dictionarySave.Add(key, new DictionaryWrapper(dicType, loadedDictionary.Keys, loadedDictionary.Values));
                }
            }
        }

        void LoadBools(BinaryReader reader)
        {
            int size = reader.ReadInt32();
            string[] boolKeys = new string[size];
            bool[] boolValues = new bool[size];

            for (int i = 0; i < size; i++) boolKeys[i] = reader.ReadString();

            if (size < 9)
            {
                BitFlag8 bitFlag = new BitFlag8(reader.ReadByte());
                for (int i = 0; i < size; i++) boolValues[i] = bitFlag == i;
            }
            else if (size < 17)
            {
                BitFlag16 bitFlag = new BitFlag16(reader.ReadUInt16());
                for (int i = 0; i < size; i++) boolValues[i] = bitFlag == i;
            }
            else if (size < 33)
            {
                BitFlag32 bitFlag = new BitFlag32(reader.ReadUInt32());
                for (int i = 0; i < size; i++) boolValues[i] = bitFlag == i;
            }
            else if (size < 65)
            {
                BitFlag64 bitFlag = new BitFlag64(reader.ReadUInt64());
                for (int i = 0; i < size; i++) boolValues[i] = bitFlag == i;
            }
            else
            {
                int bitFlagQuantity = Mathf.RoundToInt((float) size / 64f);
                for (int i = 0; i < bitFlagQuantity; i++)
                {
                    BitFlag64 bitFlag = new BitFlag64(reader.ReadUInt64());
                    for (int j = 64 * i, n = 0; n < 64; j++, n++) boolValues[j] = bitFlag == n;
                }
            }

            for (int i = 0; i < size; i++) boolSave.Add(boolKeys[i], boolValues[i]);
        }

        bool this[int index, string key] => index switch
        {
            0 => boolSave.ContainsKey(key),
            1 => shortSave.ContainsKey(key),
            2 => intSave.ContainsKey(key),
            3 => longSave.ContainsKey(key),
            4 => floatSave.ContainsKey(key),
            5 => stringSave.ContainsKey(key),
            6 => vector3Save.ContainsKey(key),
            7 => arraySave.ContainsKey(key),
            _ => dictionarySave.ContainsKey(key),
        };

        IDictionary this[int index] => index switch
        {
            0 => boolSave,
            1 => shortSave,
            2 => intSave,
            3 => longSave,
            4 => floatSave,
            5 => stringSave,
            6 => vector3Save,
            7 => arraySave,
            _ => dictionarySave,
        };

        bool CheckTag(int index, string tag)
        {
            IDictionary data = this[index];
            foreach (string key in data.Keys)
            {
                if (key.Contains(tag)) 
                    return true;
            }
            return false;
        }

        bool CheckTag(int index, string tag, out string key)
        {
            IDictionary data = this[index];
            foreach (string item in data.Keys)
            {
                if (item.Contains(tag))
                {
                    key = item;
                    return true;
                }
            }

            key = "";
            return false;
        }
    }


    #region Clases para Guardar distintos tipos de datos

    [System.Serializable]
    public sealed class Vec3
    {
        public readonly float x;
        public readonly float y;
        public readonly float z;

        public Vec3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
    }

    [System.Serializable]
    public sealed class ArrayWrapper
    {
        public System.Type type;
        public IList array;

        public ArrayWrapper(System.Type t, IList arr)
        {
            type = t;
            array = arr;
        }
    }

    [System.Serializable]
    public sealed class DictionaryWrapper
    {
        public System.Type dictionaryType;
        public List<string> keys;
        public List<dynamic> values;

        public DictionaryWrapper(System.Type dicType, ICollection _keys, ICollection _values)
        {
            dictionaryType = dicType;
            keys = new List<string>();
            values = new List<dynamic>();

            foreach(string key in _keys) keys.Add(key);
            foreach(var value in _values) values.Add(value);
        }
    }
}
#endregion
