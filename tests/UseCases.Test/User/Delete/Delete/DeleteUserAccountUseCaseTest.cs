using CommonTestUtilities.BlobStorage;
using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using MyRecipeBook.Application.UseCases.User.Delete.Delete;
using Xunit;

namespace UseCases.Test.User.Delete.Delete;
public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();

        var useCase = CreateUseCase();

        var act = async () => await useCase.Execute(user.UserIdentifier);

        await act.Should().NotThrowAsync();
    }

    private static DeleteUserAccountUseCase CreateUseCase()
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var blobStorageService = new BlobStorageServiceBuilder().Build();
        var repository = UserDeleteOnlyRepository.Build();

        return new DeleteUserAccountUseCase(repository, blobStorageService, unitOfWork);
    }
}
