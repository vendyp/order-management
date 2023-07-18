using OrderManagementApi.Domain.Enums;
using OrderManagementApi.Shared.Abstractions.Encryption;
using OrderManagementApi.Shared.Abstractions.Entities;

namespace OrderManagementApi.Domain.Entities;

public sealed class EmailSender : BaseEntity, IEntity
{
    public EmailSender()
    {
        EmailSenderId = Guid.NewGuid();
        HashedEmailSender = EmailSenderId.ToString("N");
        Status = EmailSenderStatus.Request;
    }

    public EmailSender(ISha512 sha512) : this()
    {
        HashedEmailSender = sha512.Hash(HashedEmailSender);
    }

    public Guid EmailSenderId { get; set; }

    public string HashedEmailSender { get; set; }

    /// <summary>
    /// Default value is <see cref="EmailSenderStatus.Request"/>
    /// </summary>
    public EmailSenderStatus Status { get; set; }

    public string Subject { get; set; } = null!;

    /// <summary>
    /// Mostly this value is filled with html
    /// </summary>
    public string Body { get; set; } = null!;
    
    public string Source { get; set; } = null!;
    
    public string? Template { get; set; }
    
    public string? Parameters { get; set; }
}