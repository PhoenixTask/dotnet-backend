using Application.Abstractions.Emails;
using Domain.Users;
using MediatR;

namespace Application.Users.Register;

internal sealed class UserRegisteredDomainEventHandler(IEmailService emailService) : INotificationHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await emailService.SendEmailAsync(notification.User.Email, "Greeting", "Welcome to PhoenixTask😃");
        }
        catch (Exception)
        {
            // Send email to new user if email configured properly
        }
    }
}
