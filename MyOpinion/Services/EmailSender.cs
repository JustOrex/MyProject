using MimeKit;
using MailKit.Net.Smtp;


namespace MyOpinion.Services
{
    internal class EmailSender
    {

        private string emailOfGetter;
        private string subject;
        public string Code { get; private set; }

        public EmailSender(string emailOfGetter, string subject)
        {
            Code = CreatingMessage();

            this.emailOfGetter = emailOfGetter;
            this.subject = subject;
        }

        public async Task SendEmailAsync()
        {
            using var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("МойПроект", "jonny2007xxx@yandex.ru"));
            emailMessage.To.Add(MailboxAddress.Parse(emailOfGetter));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = Code
            };
            using (var client = new SmtpClient())
            {
                try
                {

                    await client.ConnectAsync("smtp.yandex.ru", 465, true);
                    await client.AuthenticateAsync("jonny2007xxx@yandex.ru", "prisrak1320");
                    await client.SendAsync(emailMessage);


                    
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }

        private static string CreatingMessage()
        {


            var rd = new Random();
            var listOfnum = new List<string>();
            string code = null;
            
            for (int i = 0; i < 6; i++)
            {
                int a = rd.Next(0, 9);

                listOfnum.Add(a.ToString());
            }           

            foreach (var i in listOfnum)
            {
                code += i;
            }


            return code;

        }




    }
}
