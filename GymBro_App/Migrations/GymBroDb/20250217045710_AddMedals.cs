using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymBro_App.Migrations.GymBroDb
{
    /// <inheritdoc />
    public partial class AddMedals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Biometric__UserI__5AB9788F",
                table: "BiometricData");

            migrationBuilder.DropForeignKey(
                name: "FK__Challenge__Chall__690797E6",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK__Challenge__UserI__69FBBC1F",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK__GymUser__GymID__5F7E2DAC",
                table: "GymUser");

            migrationBuilder.DropForeignKey(
                name: "FK__GymUser__UserID__607251E5",
                table: "GymUser");

            migrationBuilder.DropForeignKey(
                name: "FK__Leaderboa__Chall__65370702",
                table: "Leaderboard");

            migrationBuilder.DropForeignKey(
                name: "FK__Leaderboa__UserI__662B2B3B",
                table: "Leaderboard");

            migrationBuilder.DropForeignKey(
                name: "FK__Meal__MealPlanID__5224328E",
                table: "Meal");

            migrationBuilder.DropForeignKey(
                name: "FK__MealPlan__UserID__4E53A1AA",
                table: "MealPlan");

            migrationBuilder.DropForeignKey(
                name: "FK__WorkoutPl__UserI__44CA3770",
                table: "WorkoutPlan");

            migrationBuilder.DropForeignKey(
                name: "FK__WorkoutPl__Exerc__4B7734FF",
                table: "WorkoutPlanExercise");

            migrationBuilder.DropForeignKey(
                name: "FK__WorkoutPl__Worko__4A8310C6",
                table: "WorkoutPlanExercise");

            migrationBuilder.DropTable(
                name: "FoodMeal");

            migrationBuilder.DropTable(
                name: "MealFood");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WorkoutP__66562A8B6A095EF2",
                table: "WorkoutPlanExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WorkoutP__8C51605B4BC3C9AE",
                table: "WorkoutPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK__User__1788CCAC7DFCAD03",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK__MealPlan__0620DB56A6B05EC4",
                table: "MealPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Meal__ACF6A65D7410B351",
                table: "Meal");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Leaderbo__B358A1E60F0138BA",
                table: "Leaderboard");

            migrationBuilder.DropPrimaryKey(
                name: "PK__GymUser__CB42F07CF86D83B3",
                table: "GymUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Gym__1A3A7CB679B852F4",
                table: "Gym");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Food__856DB3CBF9F6E950",
                table: "Food");

            migrationBuilder.DropPrimaryKey(
                name: "PK__FitnessC__C7AC81283F5ABD50",
                table: "FitnessChallenge");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Exercise__A074AD0F4A6FEE4D",
                table: "Exercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Challeng__16D40DE2F424EFE0",
                table: "ChallengeUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Biometri__9CF3DB06B7833220",
                table: "BiometricData");

            migrationBuilder.DropColumn(
                name: "CaloriesPerServing",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "CarbsPerServing",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "FoodName",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "ServingSize",
                table: "Food");

            migrationBuilder.RenameIndex(
                name: "UQ__User__A9D1053461D49600",
                table: "User",
                newName: "UQ__User__A9D105343E4A70F4");

            migrationBuilder.RenameColumn(
                name: "ProteinPerServing",
                table: "Food",
                newName: "MealID");

            migrationBuilder.RenameColumn(
                name: "FatPerServing",
                table: "Food",
                newName: "Amount");

            migrationBuilder.AddColumn<long>(
                name: "ApiFoodID",
                table: "Food",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdated",
                table: "BiometricData",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WorkoutP__66562A8B328513BD",
                table: "WorkoutPlanExercise",
                columns: new[] { "WorkoutPlanID", "ExerciseID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__WorkoutP__8C51605B56D8B85F",
                table: "WorkoutPlan",
                column: "WorkoutPlanID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__User__1788CCAC7E8C0468",
                table: "User",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__MealPlan__0620DB5679BCB511",
                table: "MealPlan",
                column: "MealPlanID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Meal__ACF6A65D8D571465",
                table: "Meal",
                column: "MealID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Leaderbo__B358A1E6BDFAB1ED",
                table: "Leaderboard",
                column: "LeaderboardID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GymUser__CB42F07C5698A768",
                table: "GymUser",
                columns: new[] { "GymID", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Gym__1A3A7CB61FEF4E6A",
                table: "Gym",
                column: "GymID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Food__856DB3CBFFC8A7B5",
                table: "Food",
                column: "FoodID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__FitnessC__C7AC812894125780",
                table: "FitnessChallenge",
                column: "ChallengeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Exercise__A074AD0FCA6AE41B",
                table: "Exercise",
                column: "ExerciseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Challeng__16D40DE2D78B57C9",
                table: "ChallengeUser",
                columns: new[] { "ChallengeID", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Biometri__9CF3DB0657A21018",
                table: "BiometricData",
                column: "BiometricID");

            migrationBuilder.CreateTable(
                name: "Medals",
                columns: table => new
                {
                    MedalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    StepThreshold = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Medal__A074AD0F12345678", x => x.MedalID);
                });

            migrationBuilder.CreateTable(
                name: "UserMedals",
                columns: table => new
                {
                    UserMedalID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    MedalID = table.Column<int>(type: "int", nullable: false),
                    EarnedDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserMedal__B07CE715ABCDE", x => x.UserMedalID);
                    table.ForeignKey(
                        name: "FK_UserMedal_Medal",
                        column: x => x.MedalID,
                        principalTable: "Medals",
                        principalColumn: "MedalID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMedal_User",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Food_MealID",
                table: "Food",
                column: "MealID");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedals_MedalID",
                table: "UserMedals",
                column: "MedalID");

            migrationBuilder.CreateIndex(
                name: "IX_UserMedals_UserID",
                table: "UserMedals",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK__Biometric__UserI__40058253",
                table: "BiometricData",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Challenge__Chall__4E53A1AA",
                table: "ChallengeUser",
                column: "ChallengeID",
                principalTable: "FitnessChallenge",
                principalColumn: "ChallengeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Challenge__UserI__4F47C5E3",
                table: "ChallengeUser",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Food__MealID__5224328E",
                table: "Food",
                column: "MealID",
                principalTable: "Meal",
                principalColumn: "MealID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__GymUser__GymID__44CA3770",
                table: "GymUser",
                column: "GymID",
                principalTable: "Gym",
                principalColumn: "GymID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__GymUser__UserID__45BE5BA9",
                table: "GymUser",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Leaderboa__Chall__4A8310C6",
                table: "Leaderboard",
                column: "ChallengeID",
                principalTable: "FitnessChallenge",
                principalColumn: "ChallengeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Leaderboa__UserI__4B7734FF",
                table: "Leaderboard",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Meal__MealPlanID__37703C52",
                table: "Meal",
                column: "MealPlanID",
                principalTable: "MealPlan",
                principalColumn: "MealPlanID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__MealPlan__UserID__339FAB6E",
                table: "MealPlan",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__WorkoutPl__UserI__2A164134",
                table: "WorkoutPlan",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__WorkoutPl__Exerc__30C33EC3",
                table: "WorkoutPlanExercise",
                column: "ExerciseID",
                principalTable: "Exercise",
                principalColumn: "ExerciseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__WorkoutPl__Worko__2FCF1A8A",
                table: "WorkoutPlanExercise",
                column: "WorkoutPlanID",
                principalTable: "WorkoutPlan",
                principalColumn: "WorkoutPlanID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Biometric__UserI__40058253",
                table: "BiometricData");

            migrationBuilder.DropForeignKey(
                name: "FK__Challenge__Chall__4E53A1AA",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK__Challenge__UserI__4F47C5E3",
                table: "ChallengeUser");

            migrationBuilder.DropForeignKey(
                name: "FK__Food__MealID__5224328E",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK__GymUser__GymID__44CA3770",
                table: "GymUser");

            migrationBuilder.DropForeignKey(
                name: "FK__GymUser__UserID__45BE5BA9",
                table: "GymUser");

            migrationBuilder.DropForeignKey(
                name: "FK__Leaderboa__Chall__4A8310C6",
                table: "Leaderboard");

            migrationBuilder.DropForeignKey(
                name: "FK__Leaderboa__UserI__4B7734FF",
                table: "Leaderboard");

            migrationBuilder.DropForeignKey(
                name: "FK__Meal__MealPlanID__37703C52",
                table: "Meal");

            migrationBuilder.DropForeignKey(
                name: "FK__MealPlan__UserID__339FAB6E",
                table: "MealPlan");

            migrationBuilder.DropForeignKey(
                name: "FK__WorkoutPl__UserI__2A164134",
                table: "WorkoutPlan");

            migrationBuilder.DropForeignKey(
                name: "FK__WorkoutPl__Exerc__30C33EC3",
                table: "WorkoutPlanExercise");

            migrationBuilder.DropForeignKey(
                name: "FK__WorkoutPl__Worko__2FCF1A8A",
                table: "WorkoutPlanExercise");

            migrationBuilder.DropTable(
                name: "UserMedals");

            migrationBuilder.DropTable(
                name: "Medals");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WorkoutP__66562A8B328513BD",
                table: "WorkoutPlanExercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK__WorkoutP__8C51605B56D8B85F",
                table: "WorkoutPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK__User__1788CCAC7E8C0468",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK__MealPlan__0620DB5679BCB511",
                table: "MealPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Meal__ACF6A65D8D571465",
                table: "Meal");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Leaderbo__B358A1E6BDFAB1ED",
                table: "Leaderboard");

            migrationBuilder.DropPrimaryKey(
                name: "PK__GymUser__CB42F07C5698A768",
                table: "GymUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Gym__1A3A7CB61FEF4E6A",
                table: "Gym");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Food__856DB3CBFFC8A7B5",
                table: "Food");

            migrationBuilder.DropIndex(
                name: "IX_Food_MealID",
                table: "Food");

            migrationBuilder.DropPrimaryKey(
                name: "PK__FitnessC__C7AC812894125780",
                table: "FitnessChallenge");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Exercise__A074AD0FCA6AE41B",
                table: "Exercise");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Challeng__16D40DE2D78B57C9",
                table: "ChallengeUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK__Biometri__9CF3DB0657A21018",
                table: "BiometricData");

            migrationBuilder.DropColumn(
                name: "ApiFoodID",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "LastUpdated",
                table: "BiometricData");

            migrationBuilder.RenameIndex(
                name: "UQ__User__A9D105343E4A70F4",
                table: "User",
                newName: "UQ__User__A9D1053461D49600");

            migrationBuilder.RenameColumn(
                name: "MealID",
                table: "Food",
                newName: "ProteinPerServing");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "Food",
                newName: "FatPerServing");

            migrationBuilder.AddColumn<int>(
                name: "CaloriesPerServing",
                table: "Food",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarbsPerServing",
                table: "Food",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FoodName",
                table: "Food",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServingSize",
                table: "Food",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK__WorkoutP__66562A8B6A095EF2",
                table: "WorkoutPlanExercise",
                columns: new[] { "WorkoutPlanID", "ExerciseID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__WorkoutP__8C51605B4BC3C9AE",
                table: "WorkoutPlan",
                column: "WorkoutPlanID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__User__1788CCAC7DFCAD03",
                table: "User",
                column: "UserID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__MealPlan__0620DB56A6B05EC4",
                table: "MealPlan",
                column: "MealPlanID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Meal__ACF6A65D7410B351",
                table: "Meal",
                column: "MealID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Leaderbo__B358A1E60F0138BA",
                table: "Leaderboard",
                column: "LeaderboardID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__GymUser__CB42F07CF86D83B3",
                table: "GymUser",
                columns: new[] { "GymID", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Gym__1A3A7CB679B852F4",
                table: "Gym",
                column: "GymID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Food__856DB3CBF9F6E950",
                table: "Food",
                column: "FoodID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__FitnessC__C7AC81283F5ABD50",
                table: "FitnessChallenge",
                column: "ChallengeID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Exercise__A074AD0F4A6FEE4D",
                table: "Exercise",
                column: "ExerciseID");

            migrationBuilder.AddPrimaryKey(
                name: "PK__Challeng__16D40DE2F424EFE0",
                table: "ChallengeUser",
                columns: new[] { "ChallengeID", "UserID" });

            migrationBuilder.AddPrimaryKey(
                name: "PK__Biometri__9CF3DB06B7833220",
                table: "BiometricData",
                column: "BiometricID");

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
                name: "IX_MealFood_FoodID",
                table: "MealFood",
                column: "FoodID");

            migrationBuilder.AddForeignKey(
                name: "FK__Biometric__UserI__5AB9788F",
                table: "BiometricData",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Challenge__Chall__690797E6",
                table: "ChallengeUser",
                column: "ChallengeID",
                principalTable: "FitnessChallenge",
                principalColumn: "ChallengeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Challenge__UserI__69FBBC1F",
                table: "ChallengeUser",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__GymUser__GymID__5F7E2DAC",
                table: "GymUser",
                column: "GymID",
                principalTable: "Gym",
                principalColumn: "GymID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__GymUser__UserID__607251E5",
                table: "GymUser",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Leaderboa__Chall__65370702",
                table: "Leaderboard",
                column: "ChallengeID",
                principalTable: "FitnessChallenge",
                principalColumn: "ChallengeID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Leaderboa__UserI__662B2B3B",
                table: "Leaderboard",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__Meal__MealPlanID__5224328E",
                table: "Meal",
                column: "MealPlanID",
                principalTable: "MealPlan",
                principalColumn: "MealPlanID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__MealPlan__UserID__4E53A1AA",
                table: "MealPlan",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__WorkoutPl__UserI__44CA3770",
                table: "WorkoutPlan",
                column: "UserID",
                principalTable: "User",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__WorkoutPl__Exerc__4B7734FF",
                table: "WorkoutPlanExercise",
                column: "ExerciseID",
                principalTable: "Exercise",
                principalColumn: "ExerciseID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK__WorkoutPl__Worko__4A8310C6",
                table: "WorkoutPlanExercise",
                column: "WorkoutPlanID",
                principalTable: "WorkoutPlan",
                principalColumn: "WorkoutPlanID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
