using OrderManagementApi.Core.Abstractions;
using OrderManagementApi.Domain.Entities;
using OrderManagementApi.Shared.Abstractions.Databases;
using OrderManagementApi.Shared.Abstractions.Encryption;
using OrderManagementApi.Shared.Abstractions.Serialization;

namespace OrderManagementApi.Infrastructure.Services;

public class EmailSenderService : IEmailService
{
    private readonly IDbContext _dbContext;
    private readonly ISha512 _sha512;
    private readonly IJsonSerializer _jsonSerializer;

    public EmailSenderService(IDbContext dbContext, ISha512 sha512, IJsonSerializer jsonSerializer)
    {
        _dbContext = dbContext;
        _sha512 = sha512;
        _jsonSerializer = jsonSerializer;
    }

    public async Task SendEmailAsync(Dictionary<string, string> parameters, string subject, string body, string? source)
    {
        var email = new EmailSender(_sha512)
        {
            Subject = subject,
            Body = body,
            Source = source ?? "Default"
        };

        _dbContext.Set<EmailSender>().Add(email);

        await _dbContext.SaveChangesAsync();
    }

    public async Task SendEmailWithTemplateAsync(Dictionary<string, string> parameters, string subject, string template,
        string? source)
    {
        var email = new EmailSender(_sha512)
        {
            Subject = subject,
            Body = "From template",
            Source = source ?? "Default",
            Template = template,
            Parameters = _jsonSerializer.Serialize(parameters)
        };

        _dbContext.Set<EmailSender>().Add(email);

        await _dbContext.SaveChangesAsync();
    }
}