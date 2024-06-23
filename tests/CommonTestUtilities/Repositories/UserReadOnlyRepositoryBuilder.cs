using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();

    public void ExistActiveUserWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public void GetByEmail(User user)
    {
        _repository.Setup(repository => repository.GetByEmail(user.Email)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}
