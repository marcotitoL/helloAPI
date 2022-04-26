namespace helloAPI.Services;

    public class SendGridEmailSender : SendGridService
    {
        private readonly SendgridSettings _sendgridSettings;

        public SendGridEmailSender(
            IOptions<SendgridSettings> sendgridsettings
            )
        {
            _sendgridSettings = sendgridsettings.Value;
        }


        public async Task<Response> Send(string to, string subject, string message){
            //send confirmation email
            var apiKey = _sendgridSettings.Api_key;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_sendgridSettings.Mailfrom, _sendgridSettings.Mailfromname),
                Subject = subject,
                PlainTextContent = message
            };

            msg.AddTo(new EmailAddress(to));

            return await client.SendEmailAsync(msg);

        }
    }

    public interface SendGridService
    {
        Task<Response> Send(string to, string subject, string message);
    }

    public class SendgridSettings{
        public string? Api_key {get;set;}

        public string? Mailfrom {get;set;}

        public string? Mailfromname {get;set;}

    }
