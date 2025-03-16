using Microsoft.EntityFrameworkCore;
using GymBro_App.Models;
using GymBro_App.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using GymBro_App.Services;
using GymBro_App.Helper; // Assuming EncryptionHelper is in the Helpers namespace
using GymBro_App.DAL.Abstract;
using GymBro_App.DAL.Concrete;
using System.Diagnostics;

namespace GymBro_App;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSwaggerGen();

        var connectionString = builder.Configuration.GetConnectionString("GymBroAzureConnection");

        builder.Services.AddDbContext<GymBroDbContext>(options => options
            .UseLazyLoadingProxies()
            .UseSqlServer(connectionString));

        // Add repository scopes for controllers below:
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
        builder.Services.AddScoped<IFoodRepository, FoodRepository>();
        builder.Services.AddScoped<IMealRepository, MealRepository>();
        builder.Services.AddScoped<IMealPlanRepository, MealPlanRepository>();
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IMedalRepository, MedalRepository>();
        builder.Services.AddScoped<IAwardMedalService, AwardMedalService>();
        builder.Services.AddScoped<IUserMedalRepository, UserMedalRepository>();
        builder.Services.AddScoped<IBiometricDatumRepository, BiometricDatumRepository>();
        builder.Services.AddScoped<IOAuthService, OAuthService>();  
        builder.Services.AddHttpContextAccessor(); 
        builder.Services.AddScoped<EncryptionHelper>();
        builder.Services.AddHostedService<MedalAwardingBackgroundService>();
        builder.Services.AddScoped<IGoogleMapsService, GoogleMapsService>();
        builder.Services.AddScoped<INearbySearchMapService, NearbySearchMapService>();


        

        // Configure the authentication/Identity database connection
        var authDbConnectionString = builder.Configuration.GetConnectionString("AuthGymBroAzureConnection");

        builder.Services.AddDbContext<AuthGymBroDb>(options => options
                        .UseSqlServer(authDbConnectionString));

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                        .AddEntityFrameworkStores<AuthGymBroDb>();

        string? foodApiClientId = builder.Configuration["FoodApiClientId"];
        string? foodApiClientSecret = builder.Configuration["FoodApiClientSecret"];

        if(foodApiClientId == null || foodApiClientSecret == null)
        {
            Console.WriteLine("Food API Client ID and Secret must be set in the user secrets.");
        }

        string exerciseDbAPIUrl = "https://exercisedb.p.rapidapi.com";
        string exerciseDbAPIKey = builder.Configuration["ExerciseDbApiKey"] ?? "";

        builder.Services.AddHttpClient<IFoodService, FoodService>((client, services) =>
        {
            return new FoodService(client, services.GetRequiredService<ILogger<FoodService>>(), foodApiClientId, foodApiClientSecret);
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

        // Google Maps API Configuration
        string googleMapsApiKey = builder.Configuration["GoogleMapsApiKey"] ?? "";
        
        // GoogleMapsService API Configuration
        string googleMapApiUrl = "https://maps.googleapis.com/maps/api";
        builder.Services.AddHttpClient<IGoogleMapsService, GoogleMapsService>((client, services) =>
        {
            client.BaseAddress = new Uri(googleMapApiUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-goog-api-key", googleMapsApiKey);
            return new GoogleMapsService(client, services.GetRequiredService<ILogger<GoogleMapsService>>());
        });

        // NearbySearchMapService API Configuration
        string googleNearbySearchApiUrl = "https://places.googleapis.com/v1/places:searchNearby";
        string googleNearbySearchFieldMask = "places.formattedAddress,places.displayName,places.regularOpeningHours,places.websiteUri";
        builder.Services.AddHttpClient<INearbySearchMapService, NearbySearchMapService>((client, services) =>
        {
            client.BaseAddress = new Uri(googleNearbySearchApiUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("X-Goog-Api-Key", googleMapsApiKey);
            client.DefaultRequestHeaders.Add("X-Goog-FieldMask", googleNearbySearchFieldMask);
            return new NearbySearchMapService(client, services.GetRequiredService<ILogger<NearbySearchMapService>>());
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

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30); // Keep session for 30 minutes
            options.Cookie.HttpOnly = true;                 // Protect against JavaScript access
            options.Cookie.IsEssential = true;              // Needed for GDPR compliance
        });

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

        app.UseSession();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "fitbitCallback",
            pattern: "signin-fitbit",
            defaults: new { controller = "FitbitAPI", action = "SigninFitbit" }
        );

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        // Seed the Identity database
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            await IdentityDbInitializer.SeedUsers(services);
        }

        app.Run();
    }
}
