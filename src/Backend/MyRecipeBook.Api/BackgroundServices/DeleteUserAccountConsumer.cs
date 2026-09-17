using Azure.Messaging.ServiceBus;
using MyRecipeBook.Application.UseCases.User.DeleteAccount;

namespace MyRecipeBook.Api.BackgroundServices;

public sealed class DeleteUserAccountConsumer : BackgroundService
{
    private const string QueueName = "account-deletion";

    private readonly ServiceBusProcessor _processor;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public DeleteUserAccountConsumer(ServiceBusClient serviceBusClient, IServiceScopeFactory serviceScopeFactory)
    {
        _processor = serviceBusClient.CreateProcessor(QueueName, new ServiceBusProcessorOptions());
        _serviceScopeFactory = serviceScopeFactory;
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

        using var scope = _serviceScopeFactory.CreateScope();

        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteUserAccountPermanentlyUseCase>();

        await useCase.Execute(userId);

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