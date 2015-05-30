using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using DevOne.Security.Cryptography.BCrypt;

namespace StudentFinanceSupport.Models.functions
{
    public static class  PasswordHashing
    {
        private static readonly int BCRYPT_WORK_FACTOR = 12;

        public static string Encrypt(string password)
        {

            return DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(password, DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt(BCRYPT_WORK_FACTOR));
        }

        public static bool passwordValid(string password, string dbasepassword)
        {
            try {
                bool matched = DevOne.Security.Cryptography.BCrypt.BCryptHelper.CheckPassword(password, dbasepassword);
                return matched;
            }
            catch (Exception e) {
                return false;
            }

            
        }
    }
}
