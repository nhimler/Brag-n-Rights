using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using GymBro_App.Models;

namespace GymBro_App.Areas.Identity.Data
{
    public static class IdentityDbInitializer
    {
        public static async Task SeedUsers(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<AuthGymBroDb>();
            
            // Need this to seed data that needs Identity information
            var gymBroContext = serviceProvider.GetRequiredService<GymBroDbContext>();


            context.Database.EnsureCreated();

            // Seeding only works when no other users exist. Modify with caution.
            if (!context.Users.Any())
            {
                var user1 = new IdentityUser
                {
                    UserName = "user1",
                    Email = "user1@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user1, "Password!1");

                var user2 = new IdentityUser
                {
                    UserName = "user2",
                    Email = "user2@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user2, "Password!1");

                // Insert users into the main database
                gymBroContext.Users.Add(new User
                {
                    IdentityUserId = user1.Id,
                    Username = user1.UserName,
                    FirstName = "FirstName1",
                    LastName = "LastName1",
                    Email = user1.Email,
                    Password = "hashedpassword1",
                    Age = 25,
                    Gender = "Male",
                    Weight = 75.5m,
                    Height = 180.0m,
                    FitnessLevel = "Intermediate",
                    Fitnessgoals = "Build muscle",
                    AccountCreationDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    PreferredWorkoutTime = "Morning",
                    Location = "Location1",
                    Latitude = 44.851419m,
                    Longitude = -123.237022m
                });

                gymBroContext.Users.Add(new User
                {
                    IdentityUserId = user2.Id,
                    Username = user2.UserName,
                    FirstName = "FirstName2",
                    LastName = "LastName2",
                    Email = user2.Email,
                    Password = "hashedpassword2",
                    Age = 30,
                    Gender = "Female",
                    Weight = 65.0m,
                    Height = 170.0m,
                    FitnessLevel = "Beginner",
                    Fitnessgoals = "Lose weight",
                    AccountCreationDate = DateTime.Now,
                    LastLogin = DateTime.Now,
                    PreferredWorkoutTime = "Evening",
                    Location = "Location2",
                    Latitude = 44.851419m,
                    Longitude = -123.237022m
                });

                await gymBroContext.SaveChangesAsync();
            }
        }
    }
}