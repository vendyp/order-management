namespace OrderManagementApi.Core.Abstractions;

public interface IEmailService
{
    Task SendEmailAsync(Dictionary<string, string> parameters, string subject, string body, string? source);

    Task SendEmailWithTemplateAsync(Dictionary<string, string> parameters, string subject, string template,
        string? source);
}