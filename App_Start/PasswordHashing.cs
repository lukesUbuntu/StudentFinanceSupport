using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using DevOne.Security.Cryptography.BCrypt;

namespace StudentFinanceSupport.App_Start
{
    public class PasswordHashing
    {
        private readonly int BCRYPT_WORK_FACTOR = 10;
       
        public string Encrypt(string password)
        {

            return DevOne.Security.Cryptography.BCrypt.BCryptHelper.HashPassword(password, DevOne.Security.Cryptography.BCrypt.BCryptHelper.GenerateSalt(12));
        }

        public bool passwordValid(string password,string dbasepassword)
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
