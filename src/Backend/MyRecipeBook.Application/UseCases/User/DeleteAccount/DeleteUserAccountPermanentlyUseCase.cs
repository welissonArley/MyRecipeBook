using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Storage;

namespace MyRecipeBook.Application.UseCases.User.DeleteAccount;

public class DeleteUserAccountPermanentlyUseCase : IDeleteUserAccountPermanentlyUseCase
{
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IStorageService _storageService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteUserAccountPermanentlyUseCase(
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid userId)
    {
        await _userWriteOnlyRepository.DeleteAccount(userId);

        await _unitOfWork.Commit();

        await _storageService.DeleteUserFiles(userId);
    }
}
