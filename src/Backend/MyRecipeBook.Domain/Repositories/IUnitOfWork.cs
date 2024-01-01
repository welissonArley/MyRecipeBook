namespace MyRecipeBook.Domain.Repositories;
public interface IUnitOfWork
{
    public Task Commit();
}
