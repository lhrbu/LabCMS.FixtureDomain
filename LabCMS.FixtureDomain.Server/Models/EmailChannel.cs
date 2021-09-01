
using System.Threading.Channels;
using Raccoon.Devkits.EmailToolkit;

namespace LabCMS.FixtureDomain.Server.Models;
public class EmailChannel:IDisposable
{
    private record SmtpConfig(string Host,string UserName,string Password,int Port=25);

    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailChannel> _logger;
    public EmailChannel(IConfiguration configuration,
        ILogger<EmailChannel> logger)
    {
        _configuration = configuration;
        _logger = logger;
        _ = ReaderLoopAsync();
    }

    private async Task ReaderLoopAsync()
    {
        while (!_cancelSource.Token.IsCancellationRequested)
        {
            try
            {
                SimpleEmail email = await _notificationEmails.Reader.ReadAsync();
                SmtpConfig smtpConfig = _configuration.GetValue<SmtpConfig>(nameof(SmtpConfig));
                EmailSendService sendService = new(smtpConfig.UserName, smtpConfig.Password,
                    smtpConfig.Host, smtpConfig.Port);
                await sendService.SendEmailAsync(email.FromAddress, email.ToAddress,
                    email.Subject, email.HtmlBody);
            }catch(Exception exception)
            {
                _logger.LogError(exception, "Exception in {Class}:{Method}",
                    nameof(EmailChannel),nameof(ReaderLoopAsync));
            }
        }
    }

    private readonly CancellationTokenSource _cancelSource = new();
    public void Dispose()
    {
        _cancelSource.Cancel();
        GC.SuppressFinalize(this);
    }

    public static async ValueTask AddNotificationEmailAsync(SimpleEmail email) =>
        await _notificationEmails.Writer.WriteAsync(email);

    public static async ValueTask AddNotificationEmailAsync(string fromAddress,
        IEnumerable<string> toAddresses, string subject, string htmlBody) =>
        await AddNotificationEmailAsync(new(fromAddress, toAddresses, subject, htmlBody));

    private readonly static Channel<SimpleEmail> _notificationEmails =
        Channel.CreateUnbounded<SimpleEmail>();
}
