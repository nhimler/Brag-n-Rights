using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GymBro_App.Models;

public partial class GymBroDbContext : DbContext
{
    public GymBroDbContext()
    {
    }

    public GymBroDbContext(DbContextOptions<GymBroDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Medal> Medals { get; set; }
    public virtual DbSet<UserMedal> UserMedals { get; set; }

    public virtual DbSet<BiometricDatum> BiometricData { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<FitnessChallenge> FitnessChallenges { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<Gym> Gyms { get; set; }

    public virtual DbSet<Leaderboard> Leaderboards { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MealPlan> MealPlans { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=GymBroAzureConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BiometricDatum>(entity =>
        {
            entity.HasKey(e => e.BiometricId).HasName("PK__Biometri__9CF3DB0657A21018");

            entity.HasOne(d => d.User).WithMany(p => p.BiometricData)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Biometric__UserI__40058253");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0FCA6AE41B");
        });

        modelBuilder.Entity<FitnessChallenge>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("PK__FitnessC__C7AC812894125780");

            entity.HasMany(d => d.Users).WithMany(p => p.Challenges)
                .UsingEntity<Dictionary<string, object>>(
                    "ChallengeUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Challenge__UserI__4F47C5E3"),
                    l => l.HasOne<FitnessChallenge>().WithMany()
                        .HasForeignKey("ChallengeId")
                        .HasConstraintName("FK__Challenge__Chall__4E53A1AA"),
                    j =>
                    {
                        j.HasKey("ChallengeId", "UserId").HasName("PK__Challeng__16D40DE2D78B57C9");
                        j.ToTable("ChallengeUser");
                        j.IndexerProperty<int>("ChallengeId").HasColumnName("ChallengeID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CBFFC8A7B5");

            entity.HasOne(d => d.Meal).WithMany(p => p.Foods)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Food__MealID__5224328E");
        });

        modelBuilder.Entity<Gym>(entity =>
        {
            entity.HasKey(e => e.GymId).HasName("PK__Gym__1A3A7CB61FEF4E6A");

            entity.HasMany(d => d.Users).WithMany(p => p.Gyms)
                .UsingEntity<Dictionary<string, object>>(
                    "GymUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__GymUser__UserID__45BE5BA9"),
                    l => l.HasOne<Gym>().WithMany()
                        .HasForeignKey("GymId")
                        .HasConstraintName("FK__GymUser__GymID__44CA3770"),
                    j =>
                    {
                        j.HasKey("GymId", "UserId").HasName("PK__GymUser__CB42F07C5698A768");
                        j.ToTable("GymUser");
                        j.IndexerProperty<int>("GymId").HasColumnName("GymID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A1E6BDFAB1ED");

            entity.HasOne(d => d.Challenge).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__Chall__4A8310C6");

            entity.HasOne(d => d.User).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__UserI__4B7734FF");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meal__ACF6A65D8D571465");

            entity.HasOne(d => d.MealPlan).WithMany(p => p.Meals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Meal__MealPlanID__37703C52");
        });

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(e => e.MealPlanId).HasName("PK__MealPlan__0620DB5679BCB511");

            entity.HasOne(d => d.User).WithMany(p => p.MealPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MealPlan__UserID__339FAB6E");
        });

        modelBuilder.Entity<Medal>(entity =>
        {
        entity.HasKey(e => e.MedalId).HasName("PK__Medal__A074AD0F12345678");
        });

        modelBuilder.Entity<UserMedal>(entity =>
        {
            entity.HasKey(e => e.UserMedalId).HasName("PK__UserMedal__B07CE715ABCDE");

            entity.HasOne(um => um.User)
                .WithMany(u => u.UserMedals)
                .HasForeignKey(um => um.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserMedal_User");

            entity.HasOne(um => um.Medal)
                .WithMany(m => m.UserMedals)
                .HasForeignKey(um => um.MedalId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_UserMedal_Medal");
        });


        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC7E8C0468");

            entity.Property(e => e.AccountCreationDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.WorkoutPlanId).HasName("PK__WorkoutP__8C51605B56D8B85F");

            entity.HasOne(d => d.User).WithMany(p => p.WorkoutPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__WorkoutPl__UserI__2A164134");

            entity.HasMany(d => d.Exercises).WithMany(p => p.WorkoutPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkoutPlanExercise",
                    r => r.HasOne<Exercise>().WithMany()
                        .HasForeignKey("ExerciseId")
                        .HasConstraintName("FK__WorkoutPl__Exerc__30C33EC3"),
                    l => l.HasOne<WorkoutPlan>().WithMany()
                        .HasForeignKey("WorkoutPlanId")
                        .HasConstraintName("FK__WorkoutPl__Worko__2FCF1A8A"),
                    j =>
                    {
                        j.HasKey("WorkoutPlanId", "ExerciseId").HasName("PK__WorkoutP__66562A8B328513BD");
                        j.ToTable("WorkoutPlanExercise");
                        j.IndexerProperty<int>("WorkoutPlanId").HasColumnName("WorkoutPlanID");
                        j.IndexerProperty<int>("ExerciseId").HasColumnName("ExerciseID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
