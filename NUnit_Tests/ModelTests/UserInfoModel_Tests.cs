using GymBro_App.ViewModels;
using GymBro_App.Models;
using System.ComponentModel.DataAnnotations;

namespace NUnit_Tests.ModelTests;

public class UserInfoModel_Tests
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
    
    private UserInfoModel _userInfoModel;

    [SetUp]
    public void Setup()
    {
        _userInfoModel = new UserInfoModel();
    }

    // Gender Validation
    // Valid values: Male, Female, Other
    [Test]
    [TestCase("Male")]
    [TestCase("Female")]
    [TestCase("Other")]
    public void Gender_ShouldBeValidWhenItIsMaleOrFemaleOrOther(string gender)
    {
        // Arrange
        _userInfoModel.Gender = gender;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Is.Empty);
    }

    // Gender Invalidation for invalid values
    // Valid values: Male, Female, Other
    [Test]
    [TestCase("InvalidGender")]
    public void Gender_ShouldNotBeValidWhenItIsNotMaleFemaleOrOther(string gender)
    {
        // Arrange
        _userInfoModel.Gender = gender;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Has.Count.GreaterThan(0));
    }

    // Weight Validation
    [Test]
    public void Weight_ShouldBeValidWhenItIsADecimalWithTwoDecimalPlaces()
    {
        // Arrange
        _userInfoModel.Weight = 70.50m;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Is.Empty);
    }

    // Weight Invalidation for exceeding five digits
    [Test]
    public void Weight_ShouldNotBeValidWhenItExceedsFiveDigits()
    {
        // Arrange
        _userInfoModel.Weight = 123456.78m;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Has.Count.EqualTo(1));
    }

    // Height validation
    [Test]
    public void Height_ShouldBeValidWhenItIsADecimalWithTwoDecimalPlaces()
    {
        // Arrange
        _userInfoModel.Height = 175.50m;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Is.Empty);
    }

    // Height Invalidation for exceeding five digits
    [Test]
    public void Height_ShouldNotBeValidWhenItExceedsFiveDigits()
    {
        // Arrange
        _userInfoModel.Height = 123456.78m;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Has.Count.EqualTo(1));
    }

    // FitnessLevel validation
    // Valid values: Beginner, Intermediate, Advanced
    [Test]
    [TestCase("Beginner")]
    [TestCase("Intermediate")]
    [TestCase("Advanced")]
    public void FitnessLevel_ShouldBeValidWhenItIsBeginnerIntermediateOrAdvanced(string fitnessLevel)
    {
        // Arrange
        _userInfoModel.FitnessLevel = fitnessLevel;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Is.Empty);
    }


    // FitnessLevel Invalidation for invalid values
    // Valid values: Beginner, Intermediate, Advanced
    [Test]
    [TestCase("InvalidLevel")]
    public void FitnessLevel_ShouldNotBeValidWhenItIsNotBeginnerIntermediateOrAdvanced(string fitnessLevel)
    {
        // Arrange
        _userInfoModel.FitnessLevel = fitnessLevel;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Has.Count.EqualTo(1));
    }

    // Fitnessgoals validation
    [Test]
    public void FitnessGoals_ShouldBeValidWhenItIs255CharactersOrLess()
    {
        // Arrange
        _userInfoModel.Fitnessgoals = new string('a', 255);

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Is.Empty);
    }


    // Fitnessgoals Invalidation for exceeding 255 characters
    [Test]
    public void FitnessGoals_ShouldNotBeValidWhenItExceeds255Characters()
    {
        // Arrange
        _userInfoModel.Fitnessgoals = new string('a', 256);

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Has.Count.EqualTo(1));
    }

    // Preferred Workout Time validation
    // Valid values: Morning, Afternoon, Evening
    [Test]
    [TestCase("Morning")]
    [TestCase("Afternoon")]
    [TestCase("Evening")]
    public void PreferredWorkoutTime_ShouldBeValidWhenItIsMorningAfternoonOrEvening(string workoutTime)
    {
        // Arrange
        _userInfoModel.PreferredWorkoutTime = workoutTime;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Is.Empty);
    }

    // Preferred Workout Time Invalidation for invalid values
    // Valid values: Morning, Afternoon, Evening
    [Test]
    [TestCase("Midnight")]
    public void PreferredWorkoutTime_ShouldNotBeValidWhenItIsNotMorningAfternoonOrEvening(string workoutTime)
    {
        // Arrange
        _userInfoModel.PreferredWorkoutTime = workoutTime;

        // Act
        var validationResults = ValidateModel(_userInfoModel);

        // Assert
        Assert.That(validationResults, Has.Count.EqualTo(1));
    }

    [Test]
    public void SetInfoFromUserModel_ShouldSetPropertiesInUserInfoModelToValuesFromUserModel()
    {
        // Arrange
        var user = new User
        {
            UserId = 1,
            IdentityUserId = "12",
            Age = 25,
            Gender = "Male",
            Weight = 70.5m,
            Height = 175,
            FitnessLevel = "Beginner",
            Fitnessgoals = "Weight Loss",
            PreferredWorkoutTime = "Morning",
            Username = "testuser",
            Email = "test@user.com",
            FirstName = "Test",
            LastName = "User"
        };

        // Act
        _userInfoModel.SetInfoFromUserModel(user);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_userInfoModel?.Username, Is.EqualTo(user.Username));
            Assert.That(_userInfoModel?.Email, Is.EqualTo(user.Email));
            Assert.That(_userInfoModel?.FirstName, Is.EqualTo(user.FirstName));
            Assert.That(_userInfoModel?.LastName, Is.EqualTo(user.LastName));
            Assert.That(_userInfoModel?.Age, Is.EqualTo(user.Age));
            Assert.That(_userInfoModel?.Gender, Is.EqualTo(user.Gender));
            Assert.That(_userInfoModel?.Weight, Is.EqualTo(user.Weight));
            Assert.That(_userInfoModel?.Height, Is.EqualTo(user.Height));
            Assert.That(_userInfoModel?.FitnessLevel, Is.EqualTo(user.FitnessLevel));
            Assert.That(_userInfoModel?.Fitnessgoals, Is.EqualTo(user.Fitnessgoals));
            Assert.That(_userInfoModel?.PreferredWorkoutTime, Is.EqualTo(user.PreferredWorkoutTime));
        });
        
    }
}