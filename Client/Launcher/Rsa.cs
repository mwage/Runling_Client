using System;
using System.Security.Cryptography;
using UnityEngine;

namespace Client.Scripts.Launcher
{
    public class Rsa
    {
        public string Key { get; }

        public Rsa()
        {
            try
            {
                var keyFile = Resources.Load("PublicKey") as TextAsset;
                Key = keyFile.text;
            }
            catch (Exception e)
            {
                Debug.Log("Failed to load key: " + e.Message + " - " + e.StackTrace);
            }
        }

        public byte[] Encrypt(byte[] input)
        {
            byte[] encrypted;

            using (var rsa = new RSACryptoServiceProvider(4096))
            {
                rsa.PersistKeyInCsp = false;
                rsa.FromXmlString(Key);
                encrypted = rsa.Encrypt(input, true);
            }
            return encrypted;
        }
    }
}