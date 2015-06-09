using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StudentFinanceSupport.Models.functions
{
    public class RecoveryComms
    {
        public bool sendEmail(ref Recovery theRecovery)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            theRecovery.recovery_key = this.keyCode();
            //retrieve settings from webconfig
            string FromAddress = System.Configuration.ConfigurationManager.AppSettings.Get("FromAddress");
            string SmtpServer = System.Configuration.ConfigurationManager.AppSettings.Get("SmtpServer");
            string UserName = System.Configuration.ConfigurationManager.AppSettings.Get("UserName");
            string Password = System.Configuration.ConfigurationManager.AppSettings.Get("Password");
            string EnableSSL = System.Configuration.ConfigurationManager.AppSettings.Get("EnableSSL");
            int SMTPPort = int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPPort"));
            string ReplyTo = System.Configuration.ConfigurationManager.AppSettings.Get("ReplyTo");



            var fromAddress = new MailAddress(FromAddress, "no-reply");
            var toAddress = new MailAddress(theRecovery.Administrator.Email);
            //const string fromPassword = Password;
            string subject = "Password Recovery";
            string body = String.Format("Your Reset password key is : {0}", theRecovery.recovery_key);

            var smtp = new SmtpClient
            {
                Host = SmtpServer,
                Port = SMTPPort,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(UserName, Password)

            };
            using (var message = new MailMessage(fromAddress, toAddress)

            {
                Subject = subject,
                Body = body,
                //Priority = MailPriority.High,
                //IsBodyHtml = true,
                Headers =
                {   
                    
                    //to solve spam issue add random message id
                    //soulition from http://stackoverflow.com/questions/6874964/why-emails-sent-by-net-smtpclient-are-missing-message-id
                    { "Message-ID", String.Format("<{0}@{1}>", Guid.NewGuid().ToString(), "lukes-server.com") },
                    { "X-Sender" ,FromAddress},
                    { "User-Agent" ,"ITxC# App"}
                }

            })
            {
                smtp.Send(message);
            }
            //we end here if success
            //db.Recoveries.Attach(theRecovery);
            //db.Recoveries.Attach(theRecovery);
            //db.SaveChanges();
            return true;
        }
        public bool sendSMS(ref Recovery theRecovery)
        {
            StudentRegistrationsModel db = new StudentRegistrationsModel();
            //021023517
            //021023517775
            //string mobileNumber = theRecovery.Administrator.mobile;
            //string keyCode = this.keyCode();
            string api_key = System.Configuration.ConfigurationManager.AppSettings.Get("api_key");
            string api_uri = System.Configuration.ConfigurationManager.AppSettings.Get("api_uri");

            theRecovery.recovery_key = this.keyCode();

            if (theRecovery.Administrator.mobile.Count() < 8 || theRecovery.Administrator.mobile.Count() > 13) return false;

            //string uriString = "http://home.lukes-server.com/sms/api.php";
           
            using (var client = new WebClient())
            {
                var values = new NameValueCollection
                {
                    { "api_key", api_key }, 
                    { "text", String.Format("Your Reset password key is : {0} ", theRecovery.recovery_key) },
                    { "dest",   theRecovery.Administrator.mobile }
                };
                client.QueryString = values;

                var response = client.UploadValues(api_uri, values);
                string result = System.Text.Encoding.UTF8.GetString(response);
                
                //API key will expire in 1 month after school assignment hand in
                if (result == "incorrect api key")
                    return false;

                //we end here if success

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
