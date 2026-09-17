using MyRecipeBook.Domain.Identity;
using MyRecipeBook.Domain.Messaging;
using MyRecipeBook.Domain.Repositories.User;

namespace MyRecipeBook.Application.UseCases.User.DeleteAccount;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IScheduleAccountDeletion _scheduleAccountDeletion;

    public DeleteUserAccountUseCase(
        ILoggedUser loggedUser,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IScheduleAccountDeletion scheduleAccountDeletion)
    {
        _loggedUser = loggedUser;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _scheduleAccountDeletion = scheduleAccountDeletion;
    }

    public async Task Execute()
    {
        var userId = _loggedUser.GetUserId();

        await _userWriteOnlyRepository.DeactivateAccount(userId);

        await _scheduleAccountDeletion.Schedule(userId);
    }
}
