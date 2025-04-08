using GymBro_App.ViewModels;
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
    
    private GymBro_App.ViewModels.UserInfoModel _userInfoModel;

    [SetUp]
    public void Setup()
    {
        _userInfoModel = new GymBro_App.ViewModels.UserInfoModel();
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
}