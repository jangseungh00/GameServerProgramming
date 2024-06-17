using System;
using System.Security.Cryptography;
using System.Text;

public interface SHA_256
{
    public static string CalculateSHA256(string input)
    {
        using(SHA256 sha256 = SHA256.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = sha256.ComputeHash(inputBytes);

            StringBuilder stringBuilder = new StringBuilder();  

            foreach(byte b in hashBytes)
            {
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
