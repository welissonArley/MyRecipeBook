using Moq;
using MyRecipeBook.Domain.Services.ServiceBus;

namespace CommonTestUtilities.ServiceBus;
public class DeleteUserQueueBuilder
{
    public static IDeleteUserQueue Build()
    {
        return new Mock<IDeleteUserQueue>().Object;
    }
}
