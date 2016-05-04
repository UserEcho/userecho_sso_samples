"""
    C# UserEcho Single Sign-On code example v2.0
    Date: 2016-05-04
    
    Using: 
    Check userecho_sso_test.py to generate sso_token
    Then use in your URL: http://[your_alias].userecho.com/?sso_token=sso_token
    OR in the JS widget:
    var _ues = {
        ... ,
    params:{sso_token:sso_token}
    };
"""
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
            private const int KeySize = 32;
            private static string ssoKey = "";
            private static Rijndael AES = null;

            //Generate UserEcho SSO token
            public static string GetToken(object UserAttrs, string UE_SSO_KEY)
            {
                ssoKey = UE_SSO_KEY;

                string json_string = JsonConvert.SerializeObject(UserAttrs);
                byte[] json = Encoding.UTF8.GetBytes(json_string);

                return Encrypt(json);
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
                        AES.KeySize = KeySize * 8;
                        AES.BlockSize = BlockSize * 8;
                        AES.Key = Encoding.UTF8.GetBytes(ssoKey);
                        AES.GenerateIV();
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
                    byte[] token = memoryStream.ToArray();
                    // combine iv + token
                    byte[] ivWithToken = new byte[Encryption.IV.Length + token.Length];
                    System.Buffer.BlockCopy(Encryption.IV, 0, ivWithToken, 0, Encryption.IV.Length);
                    System.Buffer.BlockCopy(token, 0, ivWithToken, Encryption.IV.Length, token.Length);
                    // convert to base64string
                    result = Convert.ToBase64String(ivWithToken);
                    return HttpUtility.UrlEncode(result);
                }
                                
               
            }



        }
    }

