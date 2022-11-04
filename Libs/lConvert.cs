using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace pyfa.auth.libs
{
    public class lConvert
    {
        public JArray convertDynamicToJArray(List<dynamic> list)
        {
            var jsonObject = new JObject();
            dynamic data = jsonObject;
            data.Lists = new JArray() as dynamic;
            dynamic detail = new JObject();
            foreach (dynamic dr in list)
            {
                detail = new JObject();
                foreach (var pair in dr)
                {
                    detail.Add(pair.Key, pair.Value);
                }
                data.Lists.Add(detail);
            }
            return data.Lists;
        }
        public List<string> convertDynamicToString(List<dynamic> dynamic)
        {
            var list = new List<string>();
            foreach (dynamic dr in dynamic)
            {
                list.Add(dr.cname);
            }
            return list;
        }


        public string TdesEncrypt(string key, string str)
        {
            string encrypted = EncryptTDes(key, str);
            return encrypted;
        }

        public string TdesDecrypt(string key, string str)
        {
            string decrypted = DecryptTdes(key, str);
            return decrypted;
        }

        private string EncryptTDes(string key, string toEncrypt)
        {
            //byte[] keyArray;

            //char byte0 = Convert.ToChar(0);
            //int numberOfByte0Needed = 8 - (toEncrypt.Length % 8);
            //for (int i = 0; i < numberOfByte0Needed && numberOfByte0Needed != 8; i++)
            //{
            //    toEncrypt += byte0;
            //}
            //byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            //MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            //keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            //TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //tdes.Key = keyArray;
            //tdes.Mode = CipherMode.ECB;
            //tdes.Padding = PaddingMode.None;

            //ICryptoTransform cTransform = tdes.CreateEncryptor();
            //byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            ////return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            //return HexEncoding.ToString(resultArray);
            string retval = "";
            return retval;
        }

        private string DecryptTdes(string key, string toDecrypt)
        {
            string retval = "";

            //byte[] keyArray;
            //char byte0 = Convert.ToChar(0);
            //int discarded;
            ////byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            //byte[] toEncryptArray = HexEncoding.GetBytes(toDecrypt, out discarded);

            //MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            //keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

            //TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //tdes.Key = keyArray;
            //tdes.Mode = CipherMode.ECB;
            //tdes.Padding = PaddingMode.None;

            //ICryptoTransform cTransform = tdes.CreateDecryptor();
            //byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            //retval = UTF8Encoding.UTF8.GetString(resultArray);
            //int i = retval.IndexOf(byte0);
            //if (i != -1)
            //{
            //    retval = retval.Substring(0, retval.IndexOf(byte0));
            //}
            return retval;
        }

        public string EncryptString(string encryptString, string EncryptionKey)
        {
            //string EncryptionKey = "idxp@rtn3rs";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Dispose();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }

        public string DecryptString(string cipherText, string EncryptionKey)
        {
            //string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Dispose();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        public string encrypt(string str)
        {
            //var key = "idxp@rtn3rs";
            var key = "idxpartners";
            string encrypted = EncryptString(str, key);
            return encrypted;
        }

        public string decrypt(string str)
        {
            //var key = "idxp@rtn3rs";
            var key = "idxpartners";
            string decrypted = DecryptString(str, key);
            return decrypted;
        }
    }
}
