using MediatR;

namespace Domain;

public sealed record NotificationGroupChange : INotification
{
    public required ModelUser Data { get; init; }

    public required EnumUserGroup PreviousGroup { get; init; }

}