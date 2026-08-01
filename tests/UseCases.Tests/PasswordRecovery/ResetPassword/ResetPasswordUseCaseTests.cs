using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Security;
using MyRecipeBook.Application.UseCases.PasswordRecovery.ResetPassword;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Exception;
using MyRecipeBook.Exception.ExceptionsBase;
using Shouldly;
using System.Net;

namespace UseCases.Tests.PasswordRecovery.ResetPassword;

public class ResetPasswordUseCaseTests
{
    [Fact]
    public async Task Success()
    {
        var (user, _) = UserBuilder.Build();
        var verificationCode = VerificationCodeBuilder.Build(user);

        var request = RequestResetPasswordJsonBuilder.Build();
        request.Email = user.Email;
        request.Code = verificationCode.Code;

        var useCase = CreateUseCase(user, verificationCode);

        await useCase.Execute(request).ShouldNotThrowAsync();
    }

    [Fact]
    public async Task ShouldThrowException_WhenUserDoesNotExist()
    {
        var (user, _) = UserBuilder.Build();
        var verificationCode = VerificationCodeBuilder.Build(user);

        var request = RequestResetPasswordJsonBuilder.Build();
        request.Code = verificationCode.Code;

        var useCase = CreateUseCase(user, verificationCode);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VERIFICATION_CODE_INVALID);
        });
    }

    [Fact]
    public async Task ShouldThrowException_WhenCodeDoesNotExist()
    {
        var (user, _) = UserBuilder.Build();
        var verificationCode = VerificationCodeBuilder.Build(user);

        var request = RequestResetPasswordJsonBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user, verificationCode);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VERIFICATION_CODE_INVALID);
        });
    }

    [Fact]
    public async Task ShouldThrowException_WhenCodeIsExpired()
    {
        var (user, _) = UserBuilder.Build();
        var verificationCode = VerificationCodeBuilder.Build(user);
        verificationCode.CreatedAt = DateTime.UtcNow.AddMinutes(-11);

        var request = RequestResetPasswordJsonBuilder.Build();
        request.Email = user.Email;
        request.Code = verificationCode.Code;

        var useCase = CreateUseCase(user, verificationCode);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VERIFICATION_CODE_INVALID);
        });
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task ShouldThrowException_WhenNewPasswordIsInvalid(int passwordLength)
    {
        var (user, _) = UserBuilder.Build();
        var verificationCode = VerificationCodeBuilder.Build(user);

        var request = RequestResetPasswordJsonBuilder.Build(passwordLength);
        request.Email = user.Email;
        request.Code = verificationCode.Code;

        var useCase = CreateUseCase(user, verificationCode);

        var exception = await useCase.Execute(request).ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetStatusCode().ShouldBe(HttpStatusCode.BadRequest);

        exception.GetErrorMessages().ShouldSatisfyAllConditions(errorMessages =>
        {
            errorMessages.Count.ShouldBe(1);
            errorMessages.ShouldContain(ResourceMessagesException.VALIDATION_PASSWORD_MIN_LENGTH);
        });
    }

    private static ResetPasswordUseCase CreateUseCase(MyRecipeBook.Domain.Entities.User user, VerificationCode verificationCode)
    {
        var userReadOnlyRepository = new IUserReadOnlyRepositoryBuilder().GetByEmail(user).Build();
        var verificationCodeReadOnlyRepository = new IVerificationCodeReadOnlyRepositoryBuilder().Get(verificationCode).Build();
        var verificationCodeWriteOnlyRepository = IVerificationCodeWriteOnlyRepositoryBuilder.Build();
        var userUpdateOnlyRepository = IUserUpdateOnlyRepositoryBuilder.Build();
        var passwordHasher = new IPasswordHasherBuilder().Build();

        return new ResetPasswordUseCase(
            userReadOnlyRepository,
            verificationCodeReadOnlyRepository,
            verificationCodeWriteOnlyRepository,
            userUpdateOnlyRepository,
            passwordHasher);
    }
}
