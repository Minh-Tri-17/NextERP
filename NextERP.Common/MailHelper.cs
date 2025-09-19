using Microsoft.Extensions.Configuration;
using NextERP.ModelBase;
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

        public static async Task<bool> SendMail(MailModel mail)
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
                        mailMessage.To.Add(new MailAddress(DataHelper.GetString(mail.To)));
                        mailMessage.Subject = DataHelper.GetString(mail.Subject);
                        mailMessage.Body = DataHelper.GetString(mail.Body);
                        mailMessage.IsBodyHtml = true;
                        mailMessage.BodyEncoding = Encoding.UTF8;

                        if (mail.CC != null)
                        {
                            foreach (var cc in mail.CC)
                            {
                                if (!string.IsNullOrWhiteSpace(cc))
                                    mailMessage.CC.Add(new MailAddress(cc));
                            }
                        }

                        if (mail.BCC != null)
                        {
                            foreach (var bcc in mail.BCC)
                            {
                                if (!string.IsNullOrWhiteSpace(bcc))
                                    mailMessage.Bcc.Add(new MailAddress(bcc));
                            }
                        }

                        if (mail.Attachments != null)
                        {
                            foreach (var formFile in mail.Attachments)
                            {
                                if (formFile.Length > 0)
                                {
                                    using var stream = formFile.OpenReadStream();
                                    var attachment = new Attachment(stream, formFile.FileName);
                                    mailMessage.Attachments.Add(attachment);
                                }
                            }
                        }

                        await smtpClient.SendMailAsync(mailMessage);
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
