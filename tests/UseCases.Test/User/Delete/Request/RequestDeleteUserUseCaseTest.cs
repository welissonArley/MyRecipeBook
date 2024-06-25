using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.ServiceBus;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Delete.Request;
using Xunit;

namespace UseCases.Test.User.Delete.Request;
public class RequestDeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();

        user.Active.Should().BeFalse();
    }

    private static RequestDeleteUserUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user)
    {
        var queue = DeleteUserQueueBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();

        return new RequestDeleteUserUseCase(queue, repository, loggedUser, unitOfWork);
    }
}
