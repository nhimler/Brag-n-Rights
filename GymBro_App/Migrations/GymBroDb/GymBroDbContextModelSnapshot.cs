﻿// <auto-generated />
using System;
using GymBro_App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymBro_App.Migrations.GymBroDb
{
    [DbContext(typeof(GymBroDbContext))]
    partial class GymBroDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ChallengeUser", b =>
                {
                    b.Property<int>("ChallengeId")
                        .HasColumnType("int")
                        .HasColumnName("ChallengeID");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("ChallengeId", "UserId")
                        .HasName("PK__Challeng__16D40DE2D78B57C9");

                    b.HasIndex("UserId");

                    b.ToTable("ChallengeUser", (string)null);
                });

            modelBuilder.Entity("ExerciseWorkoutPlan", b =>
                {
                    b.Property<int>("ExerciseId")
                        .HasColumnType("int");

                    b.Property<int>("WorkoutPlanId")
                        .HasColumnType("int");

                    b.HasKey("ExerciseId", "WorkoutPlanId");

                    b.ToTable("ExerciseWorkoutPlan");
                });

            modelBuilder.Entity("FitnessChallengeUser", b =>
                {
                    b.Property<int>("ChallengeId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ChallengeId", "UserId");

                    b.ToTable("FitnessChallengeUser");
                });

            modelBuilder.Entity("GymBro_App.Models.BiometricDatum", b =>
                {
                    b.Property<int>("BiometricId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BiometricID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BiometricId"));

                    b.Property<int?>("ActiveMinutes")
                        .HasColumnType("int");

                    b.Property<int?>("CaloriesBurned")
                        .HasColumnType("int");

                    b.Property<DateOnly?>("Date")
                        .HasColumnType("date");

                    b.Property<int?>("HeartRate")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastUpdated")
                        .HasColumnType("datetime");

                    b.Property<int?>("SleepDuration")
                        .HasColumnType("int");

                    b.Property<int?>("Steps")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("BiometricId")
                        .HasName("PK__Biometri__9CF3DB0657A21018");

                    b.HasIndex("UserId");

                    b.ToTable("BiometricData");
                });

            modelBuilder.Entity("GymBro_App.Models.Exercise", b =>
                {
                    b.Property<int>("ExerciseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ExerciseID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ExerciseId"));

                    b.Property<string>("Category")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("EquipmentRequired")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("MuscleGroup")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("Reps")
                        .HasColumnType("int");

                    b.Property<int?>("Resttime")
                        .HasColumnType("int");

                    b.Property<int?>("Sets")
                        .HasColumnType("int");

                    b.HasKey("ExerciseId")
                        .HasName("PK__Exercise__A074AD0FCA6AE41B");

                    b.ToTable("Exercise");
                });

            modelBuilder.Entity("GymBro_App.Models.FitnessChallenge", b =>
                {
                    b.Property<int>("ChallengeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ChallengeID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChallengeId"));

                    b.Property<string>("ChallengeName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("Goal")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Prize")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly?>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("ChallengeId")
                        .HasName("PK__FitnessC__C7AC812894125780");

                    b.ToTable("FitnessChallenge");
                });

            modelBuilder.Entity("GymBro_App.Models.Food", b =>
                {
                    b.Property<int>("FoodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("FoodID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FoodId"));

                    b.Property<int?>("Amount")
                        .HasColumnType("int");

                    b.Property<long?>("ApiFoodId")
                        .HasColumnType("bigint")
                        .HasColumnName("ApiFoodID");

                    b.Property<int?>("MealId")
                        .HasColumnType("int")
                        .HasColumnName("MealID");

                    b.HasKey("FoodId")
                        .HasName("PK__Food__856DB3CBFFC8A7B5");

                    b.HasIndex("MealId");

                    b.ToTable("Food");
                });

            modelBuilder.Entity("GymBro_App.Models.Gym", b =>
                {
                    b.Property<int>("GymId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("GymID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GymId"));

                    b.Property<string>("Address")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("AvailableEquipment")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("GymName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("MembershipOptions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PricingInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WebsiteUrl")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("GymId")
                        .HasName("PK__Gym__1A3A7CB61FEF4E6A");

                    b.ToTable("Gym");
                });

            modelBuilder.Entity("GymBro_App.Models.Leaderboard", b =>
                {
                    b.Property<int>("LeaderboardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("LeaderboardID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeaderboardId"));

                    b.Property<int?>("ChallengeId")
                        .HasColumnType("int")
                        .HasColumnName("ChallengeID");

                    b.Property<int?>("Rank")
                        .HasColumnType("int");

                    b.Property<int?>("Score")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("LeaderboardId")
                        .HasName("PK__Leaderbo__B358A1E6BDFAB1ED");

                    b.HasIndex("ChallengeId");

                    b.HasIndex("UserId");

                    b.ToTable("Leaderboard");
                });

            modelBuilder.Entity("GymBro_App.Models.Meal", b =>
                {
                    b.Property<int>("MealId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MealID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MealId"));

                    b.Property<DateOnly?>("Date")
                        .HasColumnType("date");

                    b.Property<string>("MealName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("MealPlanId")
                        .HasColumnType("int")
                        .HasColumnName("MealPlanID");

                    b.Property<string>("MealType")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int?>("TotalCalories")
                        .HasColumnType("int");

                    b.Property<int?>("TotalCarbs")
                        .HasColumnType("int");

                    b.Property<int?>("TotalFats")
                        .HasColumnType("int");

                    b.Property<int?>("TotalProtein")
                        .HasColumnType("int");

                    b.HasKey("MealId")
                        .HasName("PK__Meal__ACF6A65D8D571465");

                    b.HasIndex("MealPlanId");

                    b.ToTable("Meal");
                });

            modelBuilder.Entity("GymBro_App.Models.MealPlan", b =>
                {
                    b.Property<int>("MealPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MealPlanID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MealPlanId"));

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("Frequency")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PlanName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateOnly?>("StartDate")
                        .HasColumnType("date");

                    b.Property<int?>("TargetCalories")
                        .HasColumnType("int");

                    b.Property<int?>("TargetCarbs")
                        .HasColumnType("int");

                    b.Property<int?>("TargetFats")
                        .HasColumnType("int");

                    b.Property<int?>("TargetProtein")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("MealPlanId")
                        .HasName("PK__MealPlan__0620DB5679BCB511");

                    b.HasIndex("UserId");

                    b.ToTable("MealPlan");
                });

            modelBuilder.Entity("GymBro_App.Models.Medal", b =>
                {
                    b.Property<int>("MedalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("MedalID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MedalId"));

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("StepThreshold")
                        .HasColumnType("int");

                    b.HasKey("MedalId")
                        .HasName("PK__Medal__A074AD0F12345678");

                    b.ToTable("Medals");
                });

            modelBuilder.Entity("GymBro_App.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateTime?>("AccountCreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FitnessLevel")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Fitnessgoals")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Gender")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal?>("Height")
                        .HasColumnType("decimal(5, 2)");

                    b.Property<string>("IdentityUserId")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime");

                    b.Property<string>("LastName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Location")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PreferredWorkoutTime")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<byte[]>("ProfilePicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Username")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal?>("Weight")
                        .HasColumnType("decimal(5, 2)");

                    b.HasKey("UserId")
                        .HasName("PK__User__1788CCAC7E8C0468");

                    b.HasIndex(new[] { "Email" }, "UQ__User__A9D105343E4A70F4")
                        .IsUnique();

                    b.ToTable("User");
                });

            modelBuilder.Entity("GymBro_App.Models.UserMedal", b =>
                {
                    b.Property<int>("UserMedalId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("UserMedalID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserMedalId"));

                    b.Property<DateOnly>("EarnedDate")
                        .HasColumnType("date");

                    b.Property<int>("MedalId")
                        .HasColumnType("int")
                        .HasColumnName("MedalID");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("UserMedalId")
                        .HasName("PK__UserMedal__B07CE715ABCDE");

                    b.HasIndex("MedalId");

                    b.HasIndex("UserId");

                    b.ToTable("UserMedals");
                });

            modelBuilder.Entity("GymBro_App.Models.WorkoutPlan", b =>
                {
                    b.Property<int>("WorkoutPlanId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("WorkoutPlanID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("WorkoutPlanId"));

                    b.Property<string>("DifficultyLevel")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateOnly?>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("Frequency")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Goal")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("IsCompleted")
                        .HasColumnType("int");

                    b.Property<string>("PlanName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<DateOnly?>("StartDate")
                        .HasColumnType("date");

                    b.Property<int?>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("WorkoutPlanId")
                        .HasName("PK__WorkoutP__8C51605B56D8B85F");

                    b.HasIndex("UserId");

                    b.ToTable("WorkoutPlan");
                });

            modelBuilder.Entity("GymUser", b =>
                {
                    b.Property<int>("GymId")
                        .HasColumnType("int")
                        .HasColumnName("GymID");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("GymId", "UserId")
                        .HasName("PK__GymUser__CB42F07C5698A768");

                    b.HasIndex("UserId");

                    b.ToTable("GymUser", (string)null);
                });

            modelBuilder.Entity("WorkoutPlanExercise", b =>
                {
                    b.Property<int>("WorkoutPlanId")
                        .HasColumnType("int")
                        .HasColumnName("WorkoutPlanID");

                    b.Property<int>("ExerciseId")
                        .HasColumnType("int")
                        .HasColumnName("ExerciseID");

                    b.HasKey("WorkoutPlanId", "ExerciseId")
                        .HasName("PK__WorkoutP__66562A8B328513BD");

                    b.HasIndex("ExerciseId");

                    b.ToTable("WorkoutPlanExercise", (string)null);
                });

            modelBuilder.Entity("ChallengeUser", b =>
                {
                    b.HasOne("GymBro_App.Models.FitnessChallenge", null)
                        .WithMany()
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Challenge__Chall__4E53A1AA");

                    b.HasOne("GymBro_App.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Challenge__UserI__4F47C5E3");
                });

            modelBuilder.Entity("GymBro_App.Models.BiometricDatum", b =>
                {
                    b.HasOne("GymBro_App.Models.User", "User")
                        .WithMany("BiometricData")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Biometric__UserI__40058253");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymBro_App.Models.Food", b =>
                {
                    b.HasOne("GymBro_App.Models.Meal", "Meal")
                        .WithMany("Foods")
                        .HasForeignKey("MealId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Food__MealID__5224328E");

                    b.Navigation("Meal");
                });

            modelBuilder.Entity("GymBro_App.Models.Leaderboard", b =>
                {
                    b.HasOne("GymBro_App.Models.FitnessChallenge", "Challenge")
                        .WithMany("Leaderboards")
                        .HasForeignKey("ChallengeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Leaderboa__Chall__4A8310C6");

                    b.HasOne("GymBro_App.Models.User", "User")
                        .WithMany("Leaderboards")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Leaderboa__UserI__4B7734FF");

                    b.Navigation("Challenge");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymBro_App.Models.Meal", b =>
                {
                    b.HasOne("GymBro_App.Models.MealPlan", "MealPlan")
                        .WithMany("Meals")
                        .HasForeignKey("MealPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__Meal__MealPlanID__37703C52");

                    b.Navigation("MealPlan");
                });

            modelBuilder.Entity("GymBro_App.Models.MealPlan", b =>
                {
                    b.HasOne("GymBro_App.Models.User", "User")
                        .WithMany("MealPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__MealPlan__UserID__339FAB6E");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymBro_App.Models.UserMedal", b =>
                {
                    b.HasOne("GymBro_App.Models.Medal", "Medal")
                        .WithMany("UserMedals")
                        .HasForeignKey("MedalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserMedal_Medal");

                    b.HasOne("GymBro_App.Models.User", "User")
                        .WithMany("UserMedals")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_UserMedal_User");

                    b.Navigation("Medal");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymBro_App.Models.WorkoutPlan", b =>
                {
                    b.HasOne("GymBro_App.Models.User", "User")
                        .WithMany("WorkoutPlans")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .HasConstraintName("FK__WorkoutPl__UserI__2A164134");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GymUser", b =>
                {
                    b.HasOne("GymBro_App.Models.Gym", null)
                        .WithMany()
                        .HasForeignKey("GymId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__GymUser__GymID__44CA3770");

                    b.HasOne("GymBro_App.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__GymUser__UserID__45BE5BA9");
                });

            modelBuilder.Entity("WorkoutPlanExercise", b =>
                {
                    b.HasOne("GymBro_App.Models.Exercise", null)
                        .WithMany()
                        .HasForeignKey("ExerciseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__WorkoutPl__Exerc__30C33EC3");

                    b.HasOne("GymBro_App.Models.WorkoutPlan", null)
                        .WithMany()
                        .HasForeignKey("WorkoutPlanId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__WorkoutPl__Worko__2FCF1A8A");
                });

            modelBuilder.Entity("GymBro_App.Models.FitnessChallenge", b =>
                {
                    b.Navigation("Leaderboards");
                });

            modelBuilder.Entity("GymBro_App.Models.Meal", b =>
                {
                    b.Navigation("Foods");
                });

            modelBuilder.Entity("GymBro_App.Models.MealPlan", b =>
                {
                    b.Navigation("Meals");
                });

            modelBuilder.Entity("GymBro_App.Models.Medal", b =>
                {
                    b.Navigation("UserMedals");
                });

            modelBuilder.Entity("GymBro_App.Models.User", b =>
                {
                    b.Navigation("BiometricData");

                    b.Navigation("Leaderboards");

                    b.Navigation("MealPlans");

                    b.Navigation("UserMedals");

                    b.Navigation("WorkoutPlans");
                });
#pragma warning restore 612, 618
        }
    }
}
