using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace Api.Services;

public class SendgridEmailSender
{
	private readonly ILogger _logger;

	public SendgridEmailSender(IOptions<SendgridOptions> optionsAccessor,
					   ILogger<SendgridEmailSender> logger)
	{
		Options = optionsAccessor.Value;
		_logger = logger;
	}

	public SendgridOptions Options { get; }

	public async Task SendEmailAsync(string toEmail, string subject, string message)
	{
		if (string.IsNullOrEmpty(Options.Key))
		{
			throw new Exception("Null Key");
		}
		await Execute(Options.Key, subject, message, toEmail);
	}

	public async Task Execute(string apiKey, string subject, string message, string toEmail)
	{
		var client = new SendGridClient(apiKey);
		var msg = new SendGridMessage()
		{
			From = new EmailAddress(Options.SenderEmail, Options.SenderName),
			Subject = subject,
			PlainTextContent = message,
			HtmlContent = message
		};
		msg.AddTo(new EmailAddress(toEmail));

		msg.SetClickTracking(false, false);
		var response = await client.SendEmailAsync(msg);
		_logger.LogInformation(response.IsSuccessStatusCode
							   ? $"Email to {toEmail} queued successfully!"
							   : $"Failure Email to {toEmail}");
	}
}

public class SendgridOptions
{
	public static string SECTION_NAME = "Sendgrid";
	public string? Key { get; set; }
	public string SenderEmail { get; set; } = string.Empty;
	public string? SenderName { get; set; }
}