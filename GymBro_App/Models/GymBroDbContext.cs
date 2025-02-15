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
            entity.HasKey(e => e.BiometricId).HasName("PK__Biometri__9CF3DB06B7833220");

            entity.HasOne(d => d.User).WithMany(p => p.BiometricData)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Biometric__UserI__5AB9788F");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F4A6FEE4D");
        });

        modelBuilder.Entity<FitnessChallenge>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("PK__FitnessC__C7AC81283F5ABD50");

            entity.HasMany(d => d.Users).WithMany(p => p.Challenges)
                .UsingEntity<Dictionary<string, object>>(
                    "ChallengeUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Challenge__UserI__69FBBC1F"),
                    l => l.HasOne<FitnessChallenge>().WithMany()
                        .HasForeignKey("ChallengeId")
                        .HasConstraintName("FK__Challenge__Chall__690797E6"),
                    j =>
                    {
                        j.HasKey("ChallengeId", "UserId").HasName("PK__Challeng__16D40DE2F424EFE0");
                        j.ToTable("ChallengeUser");
                        j.IndexerProperty<int>("ChallengeId").HasColumnName("ChallengeID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CBF9F6E950");
        });

        modelBuilder.Entity<Gym>(entity =>
        {
            entity.HasKey(e => e.GymId).HasName("PK__Gym__1A3A7CB679B852F4");

            entity.HasMany(d => d.Users).WithMany(p => p.Gyms)
                .UsingEntity<Dictionary<string, object>>(
                    "GymUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__GymUser__UserID__607251E5"),
                    l => l.HasOne<Gym>().WithMany()
                        .HasForeignKey("GymId")
                        .HasConstraintName("FK__GymUser__GymID__5F7E2DAC"),
                    j =>
                    {
                        j.HasKey("GymId", "UserId").HasName("PK__GymUser__CB42F07CF86D83B3");
                        j.ToTable("GymUser");
                        j.IndexerProperty<int>("GymId").HasColumnName("GymID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A1E60F0138BA");

            entity.HasOne(d => d.Challenge).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__Chall__65370702");

            entity.HasOne(d => d.User).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__UserI__662B2B3B");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meal__ACF6A65D7410B351");

            entity.HasOne(d => d.MealPlan).WithMany(p => p.Meals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Meal__MealPlanID__5224328E");

            entity.HasMany(d => d.Foods).WithMany(p => p.Meals)
                .UsingEntity<Dictionary<string, object>>(
                    "MealFood",
                    r => r.HasOne<Food>().WithMany()
                        .HasForeignKey("FoodId")
                        .HasConstraintName("FK__MealFood__FoodID__57DD0BE4"),
                    l => l.HasOne<Meal>().WithMany()
                        .HasForeignKey("MealId")
                        .HasConstraintName("FK__MealFood__MealID__56E8E7AB"),
                    j =>
                    {
                        j.HasKey("MealId", "FoodId").HasName("PK__MealFood__04A07D61AD29EE52");
                        j.ToTable("MealFood");
                        j.IndexerProperty<int>("MealId").HasColumnName("MealID");
                        j.IndexerProperty<int>("FoodId").HasColumnName("FoodID");
                    });
        });

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(e => e.MealPlanId).HasName("PK__MealPlan__0620DB56A6B05EC4");

            entity.HasOne(d => d.User).WithMany(p => p.MealPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MealPlan__UserID__4E53A1AA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC7DFCAD03");

            entity.Property(e => e.AccountCreationDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.WorkoutPlanId).HasName("PK__WorkoutP__8C51605B4BC3C9AE");

            entity.HasOne(d => d.User).WithMany(p => p.WorkoutPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__WorkoutPl__UserI__44CA3770");

            entity.HasMany(d => d.Exercises).WithMany(p => p.WorkoutPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkoutPlanExercise",
                    r => r.HasOne<Exercise>().WithMany()
                        .HasForeignKey("ExerciseId")
                        .HasConstraintName("FK__WorkoutPl__Exerc__4B7734FF"),
                    l => l.HasOne<WorkoutPlan>().WithMany()
                        .HasForeignKey("WorkoutPlanId")
                        .HasConstraintName("FK__WorkoutPl__Worko__4A8310C6"),
                    j =>
                    {
                        j.HasKey("WorkoutPlanId", "ExerciseId").HasName("PK__WorkoutP__66562A8B6A095EF2");
                        j.ToTable("WorkoutPlanExercise");
                        j.IndexerProperty<int>("WorkoutPlanId").HasColumnName("WorkoutPlanID");
                        j.IndexerProperty<int>("ExerciseId").HasColumnName("ExerciseID");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
