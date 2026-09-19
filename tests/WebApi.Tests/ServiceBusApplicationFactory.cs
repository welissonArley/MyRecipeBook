namespace WebApi.Tests;

public sealed class ServiceBusApplicationFactory : MyRecipeBookApplicationFactory
{
    public override async Task InitializeAsync()
    {
        await _serviceBusContainer.StartAsync();

        await base.InitializeAsync();
    }
}