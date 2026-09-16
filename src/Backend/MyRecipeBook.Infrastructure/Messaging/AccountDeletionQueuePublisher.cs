using Azure.Messaging.ServiceBus;
using MyRecipeBook.Domain.Messaging;

namespace MyRecipeBook.Infrastructure.Messaging;

internal sealed class AccountDeletionQueuePublisher : IScheduleAccountDeletion
{
    private const string QueueName = "account-deletion";

    private readonly ServiceBusClient _serviceBusClient;

    public AccountDeletionQueuePublisher(ServiceBusClient serviceBusClient)
    {
        _serviceBusClient = serviceBusClient;
    }

    public async Task Schedule(Guid userId)
    {
        await using var sender = _serviceBusClient.CreateSender(QueueName);

        var message = new ServiceBusMessage(userId.ToString());

        await sender.SendMessageAsync(message);
    }
}
