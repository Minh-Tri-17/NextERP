using Microsoft.Extensions.Configuration;
using NextERP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NextERP.Util
{
    public class MailHelper
    {
        private static IConfiguration _config;

        public MailHelper(IConfiguration config)
        {
            _config = config;
        }

        public static bool SendMail(string toEmail, string subject, string content)
        {
            try
            {
                var host = DataHelper.GetString(_config["ConfigMail:SMTPHost"]);
                var port = DataHelper.GetInt(_config["ConfigMail:SMTPPort"]);
                var fromEmail = DataHelper.GetString(_config["ConfigMail:FromEmail"]);
                var appPassword = DataHelper.GetString(_config["ConfigMail:EmailPassword"]);
                var fromName = DataHelper.GetString(_config["ConfigMail:FromName"]);

                using (var smtpClient = new SmtpClient(host, port))
                {
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromEmail, appPassword);
                    smtpClient.Timeout = 20000;

                    using (var mailMessage = new MailMessage())
                    {
                        mailMessage.From = new MailAddress(fromEmail, fromName);
                        mailMessage.To.Add(new MailAddress(toEmail));
                        mailMessage.Subject = subject;
                        mailMessage.Body = content;
                        mailMessage.IsBodyHtml = true;
                        mailMessage.BodyEncoding = Encoding.UTF8;

                        smtpClient.Send(mailMessage);
                    }
                }

                return true;
            }
            catch (SmtpException)
            {
                return false;
            }
        }
    }
}
