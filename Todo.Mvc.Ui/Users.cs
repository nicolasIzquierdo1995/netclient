using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Mvc.Ui
{
        public static class Users
        {
            public static List<User> UsersList = new List<User>();

            public static void AddUser(string name, string password)
            {
                UsersList.Add(new User()
                {
                    Name = name,
                    Password = password
                });
            }

            public static bool Login(string name, string encodedPassword)
            {
                return UsersList.Any(x => x.Name == name && x.Password == encodedPassword);
            }

            public static string GetToken()
            {
                return ComputeSha256Hash("pasa papu");
            }

            static string ComputeSha256Hash(string rawData)
            {
                // Create a SHA256   
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
        }
}
