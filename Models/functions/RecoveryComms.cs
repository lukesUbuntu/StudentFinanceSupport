using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StudentFinanceSupport.Models.functions
{
    public class RecoveryComms
    {
        public bool sendEmail(ref Recovery theRecovery)
        {
            return true;
        }
        public bool sendSMS(ref Recovery theRecovery)
        {
            //021023517
            //021023517775
            //string mobileNumber = theRecovery.Administrator.mobile;
            //string keyCode = this.keyCode();

            theRecovery.recovery_key = this.keyCode();

            if (theRecovery.Administrator.mobile.Count() < 8 || theRecovery.Administrator.mobile.Count() > 13) return false;

            string uriString = "http://home.lukes-server.com/sms/api.php";
            //http://home.lukes-server.com/sms/api.php?user=testuser&pw=testpassword&text=%20Hi%20there%20your%20account%20code%20is%20...This%20is%20yet%20yet%20another%20test%20message%20text&dest=02102351775
            using (var client = new WebClient())
            {
                var values = new NameValueCollection
                {
                    { "api_key", "a1O3k07N21" }, 
                    { "text", "Your Reset password key is : " + theRecovery.recovery_key },
                    { "dest",   theRecovery.Administrator.mobile }
                };
                client.QueryString = values;

                var response = client.UploadValues(uriString, values);
                string result = System.Text.Encoding.UTF8.GetString(response);
                
                //API key will expire in 1 month after school assignment hand in
                if (result == "incorrect api key")
                    return false;

                return (result == "received");
                

               
            }
        }

        private string keyCode()
        {

            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var list = Enumerable.Repeat(0, 5).Select(x => chars[random.Next(chars.Length)]);
            return string.Join("", list);
        }
    }
}
