using System;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

    namespace UserEcho
    {
        public class UE_SSO
        {
            private const int BlockSize = 16;
            private static string apiKey = "";
            private static string projectKey = "";
            private static Rijndael AES = null;

            //Generate UserEcho SSO token
            public static string GetToken(object UserAttrs, string UE_API_KEY, string UE_PROJECT_KEY)
            {
                apiKey = UE_API_KEY;
                projectKey=UE_PROJECT_KEY;

                string json_string = JsonConvert.SerializeObject(UserAttrs);
                
                byte[] json = Encoding.UTF8.GetBytes(json_string);
                
                for (int i = 0; i < BlockSize; i++)
                {
                    json[i] ^= Encryption.IV[i];
                }

                return Encrypt(json);
            }


            private static byte[] Key
            {
                get
                {
                    byte[] hash = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(apiKey + projectKey));
                    byte[] key = new byte[BlockSize];
                    Array.Copy(hash, 0, key, 0, BlockSize);
                    return key;
                }
            }

            private static byte[] IV
            {
                get
                {
                    StringBuilder builder = new StringBuilder();
                    Random random = new Random();
                    for (int i = 0; i < BlockSize; i++)
                    {
                        char ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                        builder.Append(ch);
                    }

          
                    return Encoding.UTF8.GetBytes(builder.ToString());
                }
            }

            private static Rijndael Encryption
            {
                get
                {
                    if (AES == null)
                    {
                        AES = RijndaelManaged.Create();
                        
                        AES.Mode = CipherMode.CBC;
                        AES.Padding = PaddingMode.PKCS7;
                        AES.KeySize = BlockSize * 8;
                        AES.BlockSize = BlockSize * 8;
                        AES.Key = Key;
                        AES.IV = IV;
                    }
                    return AES;
                }
            }

            private static string Encrypt(byte[] json)
            {
                string result;
                using (MemoryStream memoryStream = new MemoryStream())
                {


                        ICryptoTransform encryptor = Encryption.CreateEncryptor();
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(json, 0, json.Length);
                            cryptoStream.FlushFinalBlock();

                        }
                        result = Convert.ToBase64String(memoryStream.ToArray());
                        return HttpUtility.UrlEncode(result);
                    }
                                
               
            }



        }
    }

