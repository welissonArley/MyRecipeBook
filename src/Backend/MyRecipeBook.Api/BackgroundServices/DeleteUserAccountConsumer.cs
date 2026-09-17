using Azure.Messaging.ServiceBus;

namespace MyRecipeBook.Api.BackgroundServices;

public sealed class DeleteUserAccountConsumer : BackgroundService
{
    private const string QueueName = "account-deletion";

    private readonly ServiceBusProcessor _processor;

    public DeleteUserAccountConsumer(ServiceBusClient serviceBusClient)
    {
        _processor = serviceBusClient.CreateProcessor(QueueName, new ServiceBusProcessorOptions());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _processor.ProcessMessageAsync += OnMessageAsync;
        _processor.ProcessErrorAsync += OnErrorAsync;

        await _processor.StartProcessingAsync(stoppingToken);
    }

    private async Task OnMessageAsync(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();

        var userId = Guid.Parse(body);

        //TO DO Repassar para um UseCase

        await args.CompleteMessageAsync(args.Message);
    }

    private Task OnErrorAsync(ProcessErrorEventArgs args) => Task.CompletedTask;

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await _processor.StopProcessingAsync(cancellationToken);

        await _processor.DisposeAsync();

        await base.StopAsync(cancellationToken);
    }
}