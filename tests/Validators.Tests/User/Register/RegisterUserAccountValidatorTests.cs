using MyRecipeBook.Application.UseCases.User.Register;
using MyRecipeBook.Communication.Requests;

namespace Validators.Tests.User.Register;

public class RegisterUserAccountValidatorTests
{
    [Fact]
    public void Success()
    {
        //AAA

        // Arrange

        var request = new RequestRegisterUserAccountJson
        {
            Name = "Welisson",
            Email = "welisson@gmail.com",
            Password = "123456789"
        };

        var validator = new RegisterUserAccountValidator();

        // Act

        var result = validator.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }
}
