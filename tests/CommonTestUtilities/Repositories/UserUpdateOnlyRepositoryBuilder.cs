using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository;

    public UserUpdateOnlyRepositoryBuilder() => _repository = new Mock<IUserUpdateOnlyRepository>();

    public UserUpdateOnlyRepositoryBuilder GetById(User user)
    {
        _repository.Setup(x => x.GetById(user.Id)).ReturnsAsync(user);
        return this;
    }

    public IUserUpdateOnlyRepository Build() => _repository.Object;
}
