using Microsoft.EntityFrameworkCore;
using GymBro_App.Models;
using GymBro_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using GymBro_App.Services;
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;

namespace GymBro_App;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSwaggerGen();

        // This works with user secrets. 
        // When storing the connection string in appsettings, instead use builder.GetConnectionString("GymBroConnection");
        var connectionString = builder.Configuration.GetConnectionString("GymBroAzureConnection");
        // Console.WriteLine(connectionString);
        // var connectionPassword = builder.Configuration["GymBroPassword"];

        // var connectionStringWithPassword = $"{connectionString};Password={connectionPassword}";

        builder.Services.AddDbContext<GymBroDbContext>(options => options
            .UseLazyLoadingProxies()    // Will use lazy loading, but not in LINQPad as it doesn't run Program.cs
            .UseSqlServer(connectionString));

        builder.Services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
        builder.Services.AddScoped<IFoodRepository, FoodRepository>();
        builder.Services.AddScoped<IMealRepository, MealRepository>();
        builder.Services.AddScoped<IMealPlanRepository, MealPlanRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMedalRepository, MedalRepository>();
        builder.Services.AddScoped<IAwardMedalService, AwardMedalService>();
        builder.Services.AddScoped<IUserMedalRepository, UserMedalRepository>();
        builder.Services.AddScoped<IBiometricDatumRepository, BiometricDatumRepository>();
        // Configure the authentication/Identity database connection
        var authDbConnectionString = builder.Configuration.GetConnectionString("AuthGymBroAzureConnection");

        builder.Services.AddDbContext<AuthGymBroDb>(options => options
                        .UseSqlServer(authDbConnectionString));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                        .AddEntityFrameworkStores<AuthGymBroDb>();

        string foodApiUrl = "https://platform.fatsecret.com/rest/server.api";
        string foodApiKey = builder.Configuration["FoodApiKey"] ?? "";

        string exerciseDbAPIUrl = "https://exercisedb.p.rapidapi.com";
        string exerciseDbAPIKey = builder.Configuration["ExerciseDbApiKey"];

        builder.Services.AddHttpClient<IFoodService, FoodService>((client, services) =>
        {
            client.BaseAddress = new Uri(foodApiUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", foodApiKey);
            return new FoodService(client, services.GetRequiredService<ILogger<FoodService>>());
        });

        builder.Services.AddHttpClient<IExerciseService, ExerciseService>((client, services) =>
        {
            client.BaseAddress = new Uri(exerciseDbAPIUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("x-rapidapi-host", "exercisedb.p.rapidapi.com");
            // The ExerciseDB requires the Key to be an added header instead of a new AuthenticationHeaderValue
            client.DefaultRequestHeaders.Add("x-rapidapi-key", exerciseDbAPIKey);
            var logger = services.GetRequiredService<ILogger<ExerciseService>>();
            logger.LogInformation("Request: {0} - Headers: {1}", client.BaseAddress, client.DefaultRequestHeaders);
            return new ExerciseService(client, services.GetRequiredService<ILogger<ExerciseService>>());
        });


        // Configure the Identity registration requirements
        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Sign in requirements
            options.SignIn.RequireConfirmedAccount = true;

            // Password requirements
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 10;
            options.Password.RequiredUniqueChars = 0;
        });

        // Add the dependency injections for controllers below:
        // Register the IUserRepository service with UserRepository
        builder.Services.AddScoped<IUserRepository, UserRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}
