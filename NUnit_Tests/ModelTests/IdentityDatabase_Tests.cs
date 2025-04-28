using GymBro_App.Areas.Identity.Pages.Account;
using System.ComponentModel.DataAnnotations;

namespace Model_Tests;

public class IdentityDatabase_Tests
{
    private RegisterModel.InputModel _defaultInputModel = new RegisterModel.InputModel();
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
        _defaultInputModel = new RegisterModel.InputModel
        {
            Email = "test@email.com",
            Password = "Password!1",
            ConfirmPassword = "Password!1",
            FirstName = "Test",
            LastName = "User",
            Username = "Test_User1",
        };
    }

    [Test]
    public void Email_ShouldPassValidationWhenAllEmailRequirementsAreMet()
    {
        string validEmail = "test@email.com";
        _defaultInputModel.Email = validEmail;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void Email_ShouldFailValidationWhenEmailLacksDomain()
    {
        string invalidEmail = "test.com";
        _defaultInputModel.Email = invalidEmail;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void Email_ShouldFailValidationWhenEmailIsEmpty()
    {
        string invalidEmail = "";
        _defaultInputModel.Email = invalidEmail;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldPassValidationWhenAllPasswordRequirementsAreMet()
    {
        string validPassword = "Password!1";

        _defaultInputModel.Password = validPassword;
        _defaultInputModel.ConfirmPassword = validPassword;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordIsNotAtLeast10CharactersLong()
    {
        string invalidPassword = "Pass!1";
        _defaultInputModel.Password = invalidPassword;
        _defaultInputModel.ConfirmPassword = invalidPassword;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordLacksAtLeastOneUppercase()
    {
        string invalidPassword = "password!1";
        _defaultInputModel.Password = invalidPassword;
        _defaultInputModel.ConfirmPassword = invalidPassword;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordLacksAtLeastOneLowercase()
    {
        string invalidPassword = "PASSWORD!1";
        _defaultInputModel.Password = invalidPassword;
        _defaultInputModel.ConfirmPassword = invalidPassword;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordIsEmpty()
    {
        string invalidPassword = "";
        _defaultInputModel.Password = invalidPassword;
        _defaultInputModel.ConfirmPassword = invalidPassword;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PasswordInput_ShouldFailValidationWhenPasswordLacksAtLeastSpecialCharacter()
    {
        string invalidPassword = "PASSWORD1";
        _defaultInputModel.Password = invalidPassword;
        _defaultInputModel.ConfirmPassword = invalidPassword;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldPassValidationWhenAllCharactersAreAlphabetic()
    {
        string validFirstName = "Test";

        _defaultInputModel.FirstName = validFirstName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldPassWhenFirstNameContainsHyphen()
    {
        string validFirstName = "Test-Test";

        _defaultInputModel.FirstName = validFirstName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldFailWhenFirstNameContainsNonAlphabeticOrNonHyphenCharacters()
    {
        string invalidFirstName = "Test-Test1";

        _defaultInputModel.FirstName = invalidFirstName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void FirstNameInput_ShouldFailWhenFirstNameInputIsEmpty()
    {
        string invalidFirstName = "";

        _defaultInputModel.FirstName = invalidFirstName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    
    [Test]
    public void LastNameInput_ShouldPassValidationWhenAllCharactersAreAlphabetic()
    {
        string validLastName = "Test";

        _defaultInputModel.LastName = validLastName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void LastNameInput_ShouldPassWhenLastNameContainsHyphen()
    {
        string validLastName = "Test-Test";

        _defaultInputModel.LastName = validLastName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void LastNameInput_ShouldFailWhenLastNameContainsNonAlphabeticOrNonHyphenCharacters()
    {
        string validLastName = "Test-Test1";

        _defaultInputModel.LastName = validLastName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void LastNameInput_ShouldFailValidationWhenTheLastNameInputIsEmpty()
    {
        string invalidLastName = "";

        _defaultInputModel.LastName = invalidLastName;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAllCharactersAreAlphabetic()
    {
        string validUsername = "TestUser";

        _defaultInputModel.Username = validUsername;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAllCharactersAreAlphaNumeric()
    {
        string validUsername = "TestUser1";

        _defaultInputModel.Username = validUsername;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAnyCharactersAreNonAlphaNumeric()
    {
        string validUsername = "Test_User1";

        _defaultInputModel.Username = validUsername;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldPassValidationWhenAllCharactersAreNonAlphaNumeric()
    {
        string validUsername = "@%^^$#$^*";

        _defaultInputModel.Username = validUsername;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void UsernameInput_ShouldFailValidationWhenTheUsernameInputIsEmpty()
    {
        string invalidUsername = "";

        _defaultInputModel.Username = invalidUsername;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void Age_ShouldPassValidationWhenAgeIsWithinValidRange()
    {
        int validAge = 25;

        _defaultInputModel.Age = validAge;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Empty);
    }

    [Test]
    public void Gender_ShouldFailValidationWhenGenderIsInvalid()
    {
        string invalidGender = "InvalidGender";

        _defaultInputModel.Gender = invalidGender;


        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void Weight_ShouldFailValidationWhenWeightIsOutOfRange()
    {
        decimal invalidWeight = 1000.0m;

        _defaultInputModel.Weight = invalidWeight;


        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void Height_ShouldFailValidationWhenHeightIsOutOfRange()
    {
        decimal invalidHeight = 1000.0m;

        _defaultInputModel.Height = invalidHeight;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void FitnessLevel_ShouldFailValidationWhenFitnessLevelIsInvalid()
    {
        string invalidFitnessLevel = "Expert";

        _defaultInputModel.FitnessLevel = invalidFitnessLevel;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void Fitnessgoals_ShouldFailValidationWhenFitnessGoalsExceedMaxLength()
    {
        // Fitness goals can only be 255 characters long
        string invalidFitnessGoals = new string('a', 256);

        _defaultInputModel.Fitnessgoals = invalidFitnessGoals;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }

    [Test]
    public void PreferredWorkoutTime_ShouldFailValidationWhenPreferredWorkoutTimeIsInvalid()
    {
        string invalidPreferredWorkoutTime = "Night";

        _defaultInputModel.PreferredWorkoutTime = invalidPreferredWorkoutTime;

        var validationResult = ValidateModel(_defaultInputModel);
        Assert.That(validationResult, Is.Not.Empty);
    }
}