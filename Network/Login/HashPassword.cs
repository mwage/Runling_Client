using System.Security.Cryptography;
using System.Text;

namespace Network.Login
{
    public class HashPassword
    {    
        /// <summary>
        /// Returns input hashed
        /// </summary>
        public static string ReturnHash(string input)
        {
            HashAlgorithm hashCode = SHA256.Create();
            var hashBytes = hashCode.ComputeHash(Encoding.UTF8.GetBytes(input));

            var stringBuilder = new StringBuilder();
            foreach (var element in hashBytes)
            {
                stringBuilder.Append(element.ToString("X2"));
            }
            return stringBuilder.ToString();
        }
    }
}
