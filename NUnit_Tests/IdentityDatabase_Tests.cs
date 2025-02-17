using GymBro_App.Areas.Identity.Pages.Account;
using System.ComponentModel.DataAnnotations;

namespace NUnit_Tests;

public class IdentityDatabase_Tests
{

    // ValidateModel method from Davide Bellone (https://www.code4it.dev/csharptips/unit-test-model-validation/)
    /// <summary>
    /// Validate a model
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    private static List<ValidationResult> ValidateModel(object model)
    {
        var results = new List<ValidationResult>();

        var validationContext = new ValidationContext(model, null, null);

        Validator.TryValidateObject(model, validationContext, results, true);

        if (model is IValidatableObject validatableModel)
        results.AddRange(validatableModel.Validate(validationContext));

        return results;
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Email_ShouldPassValidationWhenAllEmailRequirementsAreMet()
    {
        string validEmail = "test@email.com";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = validEmail,
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void Email_ShouldFailValidationWhenEmailLacksDomain()
    {
        string invalidEmail = "test.com";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = invalidEmail,
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void Email_ShouldFailValidationWhenEmailIsEmpty()
    {
        string invalidEmail = "";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = invalidEmail,
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldPassValidationWhenAllPasswordRequirementsAreMet()
    {
        string validPassword = "Password!1";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = validPassword,
            ConfirmPassword = validPassword,
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordIsNotAtLeast10CharactersLong()
    {
        string invalidPassword = "Pass!1";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = invalidPassword,
            ConfirmPassword = invalidPassword,
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordLacksAtLeastOneUppercase()
    {
        string invalidPassword = "password!1";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = invalidPassword,
            ConfirmPassword = invalidPassword,
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordLacksAtLeastOneLowercase()
    {
        string invalidPassword = "PASSWORD!1";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = invalidPassword,
            ConfirmPassword = invalidPassword,
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordIsEmpty()
    {
        string invalidPassword = "";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = invalidPassword,
            ConfirmPassword = invalidPassword,
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordLacksAtLeastSpecialCharacter()
    {
        string invalidPassword = "PASSWORD1";
        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = invalidPassword,
            ConfirmPassword = invalidPassword,
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldPassValidationWhenAllCharactersAreAlphabetic()
    {
        string validFirstName = "Test";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = validFirstName,
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldPassWhenFirstNameContainsHyphen()
    {
        string validFirstName = "Test-Test";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = validFirstName,
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldFailWhenFirstNameContainsNonAlphabeticOrNonHyphenCharacters()
    {
        string invalidFirstName = "Test-Test1";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = invalidFirstName,
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldFailWhenFirstNameInputIsEmpty()
    {
        string invalidFirstName = "";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = invalidFirstName,
            LastName = "User",
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    
    [Test]
    public void LastNameInput_ShouldPassValidationWhenAllCharactersAreAlphabetic()
    {
        string validLastName = "Test";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = validLastName,
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void LastNameInput_ShouldPassWhenLastNameContainsHyphen()
    {
        string validLastName = "Test-Test";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = validLastName,
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void LastNameInput_ShouldFailWhenLastNameContainsNonAlphabeticOrNonHyphenCharacters()
    {
        string validLastName = "Test-Test1";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = validLastName,
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void LastNameInput_ShouldFailValidationWhenTheLastNameInputIsEmpty()
    {
        string invalidLastName = "";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = invalidLastName,
            Username = "Test_User1"
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAllCharactersAreAlphabetic()
    {
        string validUsername = "TestUser";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = validUsername
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAllCharactersAreAlphaNumeric()
    {
        string validUsername = "TestUser1";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = validUsername
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAnyCharactersAreNonAlphaNumeric()
    {
        string validUsername = "Test_User1";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = validUsername
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAllCharactersAreNonAlphaNumeric()
    {
        string validUsername = "@%^^$#$^*";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = validUsername
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldFailValidationWhenTheUsernameInputIsEmpty()
    {
        string invalidUsername = "";

        RegisterModel.InputModel input = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = invalidUsername
        };

        var validationResult = ValidateModel(input);
        Assert.That(validationResult, Is.Not.Empty);
    }
}