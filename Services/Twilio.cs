namespace helloAPI.Services;

    public class TwilioSMSSender : TwilioService
    {
        private readonly TwilioSettings _twilioSettings;

        public TwilioSMSSender(
            IOptions<TwilioSettings> twilioSettings
            )
        {
            _twilioSettings= twilioSettings.Value;
        }


        public async Task<MessageResource> Send(string to, string sms){
           

            TwilioClient.Init(_twilioSettings.SID, _twilioSettings.Auth_token);

            MessageResource messageResource;

            try{
                messageResource = await MessageResource.CreateAsync(
                    body: sms ,
                    from: new Twilio.Types.PhoneNumber(_twilioSettings.PhoneNumber),
                    to: new Twilio.Types.PhoneNumber( to )
                );
            }
            catch{
                throw;
            }

            return messageResource;

        }
    }

    public interface TwilioService
    {
        Task<MessageResource> Send(string to, string sms);
    }

    public class TwilioSettings{
        public string? SID {get;set;}

        public string? Auth_token {get;set;}

        public string? PhoneNumber {get;set;}

    }
