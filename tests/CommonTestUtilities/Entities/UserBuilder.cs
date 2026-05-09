using Bogus;
using CommonTestUtilities.Security;
using MyRecipeBook.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static (User user, string password) Build()
    {
        var (password, passwordHashed) = GenerateRandomPassword();

        var user = new Faker<User>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, _ => passwordHashed);

        return (user, password);
    }

    private static (string password, string passwordHashed) GenerateRandomPassword()
    {
        var passwordEncripter = new IPasswordHasherBuilder().Build();

        var password = new Faker().Internet.Password();
        
        return (password, passwordEncripter.HashPassword(password));
    }
}
