namespace MyRecipeBook.Domain.Repositories;

public interface IUnitOfWork
{
    Task Commit();
}
