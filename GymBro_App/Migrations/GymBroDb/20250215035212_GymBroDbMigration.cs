using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBro_App.Migrations.GymBroDb
{
    /// <inheritdoc />
    public partial class GymBroDbMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercise",
                columns: table => new
                {
                    ExerciseID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Category = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    MuscleGroup = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EquipmentRequired = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Reps = table.Column<int>(type: "int", nullable: true),
                    Sets = table.Column<int>(type: "int", nullable: true),
                    Resttime = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Exercise__A074AD0F4A6FEE4D", x => x.ExerciseID);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseWorkoutPlan",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseWorkoutPlan", x => new { x.ExerciseId, x.WorkoutPlanId });
                });

            migrationBuilder.CreateTable(
                name: "FitnessChallenge",
                columns: table => new
                {
                    ChallengeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallengeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Goal = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Prize = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__FitnessC__C7AC81283F5ABD50", x => x.ChallengeID);
                });

            migrationBuilder.CreateTable(
                name: "FitnessChallengeUser",
                columns: table => new
                {
                    ChallengeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FitnessChallengeUser", x => new { x.ChallengeId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    FoodID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CaloriesPerServing = table.Column<int>(type: "int", nullable: true),
                    ProteinPerServing = table.Column<int>(type: "int", nullable: true),
                    CarbsPerServing = table.Column<int>(type: "int", nullable: true),
                    FatPerServing = table.Column<int>(type: "int", nullable: true),
                    ServingSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Food__856DB3CBF9F6E950", x => x.FoodID);
                });

            migrationBuilder.CreateTable(
                name: "FoodMeal",
                columns: table => new
                {
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    MealId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodMeal", x => new { x.FoodId, x.MealId });
                });

            migrationBuilder.CreateTable(
                name: "Gym",
                columns: table => new
                {
                    GymID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GymName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AvailableEquipment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PricingInfo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembershipOptions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Gym__1A3A7CB679B852F4", x => x.GymID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdentityUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Age = table.Column<int>(type: "int", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Weight = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    Height = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    FitnessLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fitnessgoals = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountCreationDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    LastLogin = table.Column<DateTime>(type: "datetime", nullable: true),
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PreferredWorkoutTime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__User__1788CCAC7DFCAD03", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "BiometricData",
                columns: table => new
                {
                    BiometricID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    Steps = table.Column<int>(type: "int", nullable: true),
                    CaloriesBurned = table.Column<int>(type: "int", nullable: true),
                    HeartRate = table.Column<int>(type: "int", nullable: true),
                    SleepDuration = table.Column<int>(type: "int", nullable: true),
                    ActiveMinutes = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Biometri__9CF3DB06B7833220", x => x.BiometricID);
                    table.ForeignKey(
                        name: "FK__Biometric__UserI__5AB9788F",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChallengeUser",
                columns: table => new
                {
                    ChallengeID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Challeng__16D40DE2F424EFE0", x => new { x.ChallengeID, x.UserID });
                    table.ForeignKey(
                        name: "FK__Challenge__Chall__690797E6",
                        column: x => x.ChallengeID,
                        principalTable: "FitnessChallenge",
                        principalColumn: "ChallengeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Challenge__UserI__69FBBC1F",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GymUser",
                columns: table => new
                {
                    GymID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__GymUser__CB42F07CF86D83B3", x => new { x.GymID, x.UserID });
                    table.ForeignKey(
                        name: "FK__GymUser__GymID__5F7E2DAC",
                        column: x => x.GymID,
                        principalTable: "Gym",
                        principalColumn: "GymID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__GymUser__UserID__607251E5",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Leaderboard",
                columns: table => new
                {
                    LeaderboardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChallengeID = table.Column<int>(type: "int", nullable: true),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    Rank = table.Column<int>(type: "int", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Leaderbo__B358A1E60F0138BA", x => x.LeaderboardID);
                    table.ForeignKey(
                        name: "FK__Leaderboa__Chall__65370702",
                        column: x => x.ChallengeID,
                        principalTable: "FitnessChallenge",
                        principalColumn: "ChallengeID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__Leaderboa__UserI__662B2B3B",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealPlan",
                columns: table => new
                {
                    MealPlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    PlanName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TargetCalories = table.Column<int>(type: "int", nullable: true),
                    TargetProtein = table.Column<int>(type: "int", nullable: true),
                    TargetCarbs = table.Column<int>(type: "int", nullable: true),
                    TargetFats = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MealPlan__0620DB56A6B05EC4", x => x.MealPlanID);
                    table.ForeignKey(
                        name: "FK__MealPlan__UserID__4E53A1AA",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlan",
                columns: table => new
                {
                    WorkoutPlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    PlanName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Frequency = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Goal = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsCompleted = table.Column<int>(type: "int", nullable: true),
                    DifficultyLevel = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkoutP__8C51605B4BC3C9AE", x => x.WorkoutPlanID);
                    table.ForeignKey(
                        name: "FK__WorkoutPl__UserI__44CA3770",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MealType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Date = table.Column<DateOnly>(type: "date", nullable: true),
                    TotalCalories = table.Column<int>(type: "int", nullable: true),
                    TotalProtein = table.Column<int>(type: "int", nullable: true),
                    TotalCarbs = table.Column<int>(type: "int", nullable: true),
                    TotalFats = table.Column<int>(type: "int", nullable: true),
                    MealPlanID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Meal__ACF6A65D7410B351", x => x.MealID);
                    table.ForeignKey(
                        name: "FK__Meal__MealPlanID__5224328E",
                        column: x => x.MealPlanID,
                        principalTable: "MealPlan",
                        principalColumn: "MealPlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlanExercise",
                columns: table => new
                {
                    WorkoutPlanID = table.Column<int>(type: "int", nullable: false),
                    ExerciseID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkoutP__66562A8B6A095EF2", x => new { x.WorkoutPlanID, x.ExerciseID });
                    table.ForeignKey(
                        name: "FK__WorkoutPl__Exerc__4B7734FF",
                        column: x => x.ExerciseID,
                        principalTable: "Exercise",
                        principalColumn: "ExerciseID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__WorkoutPl__Worko__4A8310C6",
                        column: x => x.WorkoutPlanID,
                        principalTable: "WorkoutPlan",
                        principalColumn: "WorkoutPlanID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MealFood",
                columns: table => new
                {
                    MealID = table.Column<int>(type: "int", nullable: false),
                    FoodID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MealFood__04A07D61AD29EE52", x => new { x.MealID, x.FoodID });
                    table.ForeignKey(
                        name: "FK__MealFood__FoodID__57DD0BE4",
                        column: x => x.FoodID,
                        principalTable: "Food",
                        principalColumn: "FoodID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__MealFood__MealID__56E8E7AB",
                        column: x => x.MealID,
                        principalTable: "Meal",
                        principalColumn: "MealID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BiometricData_UserID",
                table: "BiometricData",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeUser_UserID",
                table: "ChallengeUser",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_GymUser_UserID",
                table: "GymUser",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboard_ChallengeID",
                table: "Leaderboard",
                column: "ChallengeID");

            migrationBuilder.CreateIndex(
                name: "IX_Leaderboard_UserID",
                table: "Leaderboard",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_MealPlanID",
                table: "Meal",
                column: "MealPlanID");

            migrationBuilder.CreateIndex(
                name: "IX_MealFood_FoodID",
                table: "MealFood",
                column: "FoodID");

            migrationBuilder.CreateIndex(
                name: "IX_MealPlan_UserID",
                table: "MealPlan",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UQ__User__A9D1053461D49600",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlan_UserID",
                table: "WorkoutPlan",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlanExercise_ExerciseID",
                table: "WorkoutPlanExercise",
                column: "ExerciseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BiometricData");

            migrationBuilder.DropTable(
                name: "ChallengeUser");

            migrationBuilder.DropTable(
                name: "ExerciseWorkoutPlan");

            migrationBuilder.DropTable(
                name: "FitnessChallengeUser");

            migrationBuilder.DropTable(
                name: "FoodMeal");

            migrationBuilder.DropTable(
                name: "GymUser");

            migrationBuilder.DropTable(
                name: "Leaderboard");

            migrationBuilder.DropTable(
                name: "MealFood");

            migrationBuilder.DropTable(
                name: "WorkoutPlanExercise");

            migrationBuilder.DropTable(
                name: "Gym");

            migrationBuilder.DropTable(
                name: "FitnessChallenge");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "Exercise");

            migrationBuilder.DropTable(
                name: "WorkoutPlan");

            migrationBuilder.DropTable(
                name: "MealPlan");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
