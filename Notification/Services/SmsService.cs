using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Notification.Services;

public class SmsService
{
    private readonly string _fromPhoneNumber;

    public SmsService(string accountSid, string authToken, string fromPhoneNumber)
    {
        TwilioClient.Init(accountSid, authToken);
        _fromPhoneNumber = fromPhoneNumber;
    }

    public void SendSms(string to, string body)
    {
        var message = MessageResource.Create(
            body: body,
            from: new Twilio.Types.PhoneNumber(_fromPhoneNumber),
            to: new Twilio.Types.PhoneNumber(to)
        );
    }
}
