using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Net.Sockets;

namespace TestClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            List<string> ipv4 = new();
            List<string> ipv6 = new();
            foreach (var ip in ipEntry.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipv4.Add(ip.ToString());
                }
                else
                {
                    if (!ip.IsIPv6LinkLocal)
                    {
                        ipv6.Add(ip.ToString());
                    }
                }
            }
            string messageText = string.Empty;
            messageText += "ipv4: \n";
            foreach (var ip in ipv4)
            {
                messageText += ip;
                messageText += "\n";
            }
            messageText += "\n";
            messageText += "ipv6: \n";
            foreach (var ip in ipv6)
            {
                messageText += ip;
                messageText += "\n";
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("GFeonix", "gyz8810@qq.com"));
            message.To.Add(new MailboxAddress("GFeonix", "gzy8810@qq.com"));
            message.Subject = "IP Changed";

            message.Body = new TextPart("plain")
            {
                Text = messageText
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp.qq.com", 587, false);

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("gyz8810", "fguqihrwihqwhhij");

                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}