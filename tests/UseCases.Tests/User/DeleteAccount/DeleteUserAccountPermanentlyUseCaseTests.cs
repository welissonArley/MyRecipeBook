using CommonTestUtilities.Repositories;
using CommonTestUtilities.Storage;
using MyRecipeBook.Application.UseCases.User.DeleteAccount;
using Shouldly;

namespace UseCases.Tests.User.DeleteAccount;

public class DeleteUserAccountPermanentlyUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var useCase = CreateUseCase();

        await useCase.Execute(Guid.NewGuid()).ShouldNotThrowAsync();
    }

    private static DeleteUserAccountPermanentlyUseCase CreateUseCase()
    {
        var userWriteOnlyRepository = IUserWriteOnlyRepositoryBuilder.Build();
        var storageService = IStorageServiceBuilder.Build();
        var unitOfWork = IUnitOfWorkBuilder.Build();

        return new DeleteUserAccountPermanentlyUseCase(userWriteOnlyRepository, storageService, unitOfWork);
    }
}
