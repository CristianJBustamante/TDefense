using UnityEngine;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace com.Pizia.Saver
{
    public static class Saver
    {
        static string filePath = Application.persistentDataPath + "/save.dat";
        static string key = "pizia101"; // Solo soporta 8 carectares ni mas ni menos 8-bit

        public static void Save(DataSave save) // Crea un archivo, pasa a binario la DATA, lo serializa y cierra el archivo, luego lo encripta 
        {
            FileStream file = File.Create(filePath);
            try
            {
                //BinaryFormatter bf = new BinaryFormatter();
                //DataSave data = save;
                //bf.Serialize(file, save);
                BinaryWriter binaryWriter = new BinaryWriter(file);
                save.Serialize(binaryWriter);
                file.Close();
                EncryptFile(filePath, key);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error while saving file: {e.Message}\n{e.StackTrace}");
                file.Close();
            }
        }

        public static DataSave Load() //Busca un archivo que se encuentre en el filePath, lo desencripta, abre la caperta, lo deserializa y devuelve la DATA
        {
            if (File.Exists(filePath))
            {
                //BinaryFormatter bf = new BinaryFormatter();
                //FileStream file = File.Open(filePath, FileMode.Open);
                using MemoryStream ms = DencryptFile(filePath, key);
                ms.Seek(0, SeekOrigin.Begin);
                BinaryReader reader = new BinaryReader(ms);
                DataSave data = new DataSave();
                data.Deserialize(reader);
                return data;
            }
            else
            {
                return new DataSave();
            }

        }

        public static void EncryptFile(string filePath, string key)
        {
            byte[] plainContent = File.ReadAllBytes(filePath);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(),
                        CryptoStreamMode.Write);

                    cryptoStream.Write(plainContent, 0, plainContent.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(filePath, memStream.ToArray());
                }
            }
        }

        public static MemoryStream DencryptFile(string filePath, string key)
        {
            byte[] plainContent = File.ReadAllBytes(filePath);
            MemoryStream memStream = new MemoryStream();
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateDecryptor(),
                        CryptoStreamMode.Write);

                cryptoStream.Write(plainContent, 0, plainContent.Length);
                cryptoStream.FlushFinalBlock();
                //cryptoStream.Close();
                //File.WriteAllBytes(filePath, memStream.ToArray());
            }

            return memStream;
        }

        [ConsoleFunction]
        public static void Clear()
        {
            if (Application.isPlaying) SaveManager.Clear();
            try { File.Delete(filePath); }
            catch (Exception e) { Debug.LogError("Error: " + e.Message); }
        }
    }
}
