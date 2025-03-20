using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
namespace cimenOtoV11.yeniSiparisBilgi
{
    public class MailHelper
    {
        public static void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                string fromEmail = "emrebudak05052021@gmail.com";  // Gönderici e-posta adresi
                string password = "sxxv ujiw rdsp zgxp";       // Gmail uygulama şifresi

                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail); // Alıcı adresi

                smtpClient.Send(mailMessage); // Mail gönder
            }
            catch (Exception ex)
            {
                // Hata yönetimi
                Console.WriteLine("Mail gönderme hatası: " + ex.Message);
            }
        }
    }
}