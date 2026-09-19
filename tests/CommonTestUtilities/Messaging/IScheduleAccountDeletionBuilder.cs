using Moq;
using MyRecipeBook.Domain.Messaging;

namespace CommonTestUtilities.Messaging;

public class IScheduleAccountDeletionBuilder
{
    public static IScheduleAccountDeletion Build()
    {
        var mock = new Mock<IScheduleAccountDeletion>();

        return mock.Object;
    }
}
