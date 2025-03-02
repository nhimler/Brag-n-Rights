// using Microsoft.AspNetCore.Mvc.Testing;
// using GymBro_App;
using Moq;
using Microsoft.EntityFrameworkCore;
using GymBro_App.Models;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;

namespace Repository_Tests;

[TestFixture]
public class MealPlanRepository_Tests
{
    private Mock<MealPlanRepository> _mealPlanRepositoryMock;
    private Mock<GymBroDbContext> _contextMock;

    [SetUp]
    public void Setup()
    {
        _contextMock = new Mock<GymBroDbContext>();
        _contextMock.Setup(c => c.MealPlans).Returns(Mock.Of<DbSet<MealPlan>>());
        _mealPlanRepositoryMock = new Mock<MealPlanRepository>(_contextMock.Object);
    }

    [Test]
    public void Test_GetFirstMealPlanForUser()
    {
        IQueryable<MealPlan> mealPlans = new List<MealPlan>
        {
            new MealPlan { MealPlanId = 1, UserId = 1 },
            new MealPlan { MealPlanId = 2, UserId = 2 },
            new MealPlan { MealPlanId = 3, UserId = 1 }
        }.AsQueryable();
        _mealPlanRepositoryMock.Setup(mp => mp.GetAll()).Returns(mealPlans);

        var result = _mealPlanRepositoryMock.Object.GetFirstMealPlanForUser(1);

        Assert.That(result?.MealPlanId, Is.EqualTo(1));

        result = _mealPlanRepositoryMock.Object.GetFirstMealPlanForUser(2);

        Assert.That(result?.MealPlanId, Is.EqualTo(2));

        result = _mealPlanRepositoryMock.Object.GetFirstMealPlanForUser(3);

        Assert.That(result?.MealPlanId, Is.EqualTo(null));
    }

    [Test]
    public void Test_GetMealPlansForUser()
    {
        IQueryable<MealPlan> mealPlans = new List<MealPlan>
        {
            new MealPlan { MealPlanId = 1, UserId = 1 },
            new MealPlan { MealPlanId = 2, UserId = 2 },
            new MealPlan { MealPlanId = 3, UserId = 1 }
        }.AsQueryable();
        _mealPlanRepositoryMock.Setup(mp => mp.GetAll()).Returns(mealPlans);

        var result = _mealPlanRepositoryMock.Object.GetMealPlansForUser(1);

        Assert.That(result?.Count(), Is.EqualTo(2));

        result = _mealPlanRepositoryMock.Object.GetMealPlansForUser(2);

        Assert.That(result?.Count(), Is.EqualTo(1));

        result = _mealPlanRepositoryMock.Object.GetMealPlansForUser(3);

        Assert.That(result?.Count(), Is.EqualTo(0));
    }

    [Test]
    public void Test_HasMeals()
    {
        MealPlan mealPlan = new MealPlan
        {
            MealPlanId = 1,
            Meals = new List<Meal>
            {
                new Meal { MealId = 1 },
                new Meal { MealId = 2 },
                new Meal { MealId = 3 }
            }
        };
        _mealPlanRepositoryMock.Setup(mp => mp.FindById(1)).Returns(mealPlan);
        
        var result = _mealPlanRepositoryMock.Object.HasMeals(1);

        Assert.That(result, Is.True);

        result = _mealPlanRepositoryMock.Object.HasMeals(0);

        Assert.That(result, Is.False);
    }

    [Test]
    public void Test_FirstMeal()
    {
        MealPlan mealPlan = new MealPlan
        {
            MealPlanId = 1,
            Meals = new List<Meal>
            {
                new Meal { MealId = 1 },
                new Meal { MealId = 2 },
                new Meal { MealId = 3 }
            }
        };
        _mealPlanRepositoryMock.Setup(mp => mp.FindById(1)).Returns(mealPlan);

        var result = _mealPlanRepositoryMock.Object.FirstMeal(1);

        Assert.That(result?.MealId, Is.EqualTo(1));

        result = _mealPlanRepositoryMock.Object.FirstMeal(0);

        Assert.That(result, Is.Null);
    }
}

