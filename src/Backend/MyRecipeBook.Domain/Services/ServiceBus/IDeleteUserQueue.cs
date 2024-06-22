using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Services.ServiceBus;
public interface IDeleteUserQueue
{
    Task SendMessage(User user);
}
