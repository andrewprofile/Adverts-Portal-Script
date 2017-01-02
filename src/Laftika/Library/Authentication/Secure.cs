using System;
using System.Security.Cryptography;
using System.Text;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Laftika.Library.Authentication
{
    public class Secure
    {
        public static string HashString(string password)
        {
            var hashPassword = SHA1.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = hashPassword.ComputeHash(bytes);

            return Convert.ToBase64String(hashBytes);
        }

        public static string CreateKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new string(stringChars);
        }
    }
}
