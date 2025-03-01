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

    public virtual DbSet<Medal> Medals { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMedal> UserMedals { get; set; }

    public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }

    public DbSet<TokenEntity> Tokens { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=GymBroAzureConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BiometricDatum>(entity =>
        {
            entity.HasKey(e => e.BiometricId).HasName("PK__Biometri__9CF3DB06ACEB0EE9");

            entity.HasOne(d => d.User).WithMany(p => p.BiometricData).HasConstraintName("FK__Biometric__UserI__4F47C5E3");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F7ED94F8D");
        });

        modelBuilder.Entity<FitnessChallenge>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("PK__FitnessC__C7AC81286253C2F9");

            entity.HasMany(d => d.Users).WithMany(p => p.Challenges)
                .UsingEntity<Dictionary<string, object>>(
                    "ChallengeUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Challenge__UserI__43D61337"),
                    l => l.HasOne<FitnessChallenge>().WithMany()
                        .HasForeignKey("ChallengeId")
                        .HasConstraintName("FK__Challenge__Chall__42E1EEFE"),
                    j =>
                    {
                        j.HasKey("ChallengeId", "UserId").HasName("PK__Challeng__16D40DE2FB7FF4EE");
                        j.ToTable("ChallengeUser");
                        j.IndexerProperty<int>("ChallengeId").HasColumnName("ChallengeID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CB1BA8FF92");

            entity.HasOne(d => d.Meal).WithMany(p => p.Foods)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Food__MealID__46B27FE2");
        });

        modelBuilder.Entity<Gym>(entity =>
        {
            entity.HasKey(e => e.GymId).HasName("PK__Gym__1A3A7CB623058297");

            entity.HasMany(d => d.Users).WithMany(p => p.Gyms)
                .UsingEntity<Dictionary<string, object>>(
                    "GymUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__GymUser__UserID__3A4CA8FD"),
                    l => l.HasOne<Gym>().WithMany()
                        .HasForeignKey("GymId")
                        .HasConstraintName("FK__GymUser__GymID__395884C4"),
                    j =>
                    {
                        j.HasKey("GymId", "UserId").HasName("PK__GymUser__CB42F07C70D62B18");
                        j.ToTable("GymUser");
                        j.IndexerProperty<int>("GymId").HasColumnName("GymID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A1E6AD110D05");

            entity.HasOne(d => d.Challenge).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__Chall__3F115E1A");

            entity.HasOne(d => d.User).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__UserI__40058253");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meal__ACF6A65D9E8136AC");

            entity.HasOne(d => d.MealPlan).WithMany(p => p.Meals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Meal__MealPlanID__2BFE89A6");
        });

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(e => e.MealPlanId).HasName("PK__MealPlan__0620DB562D4300E1");

            entity.HasOne(d => d.User).WithMany(p => p.MealPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MealPlan__UserID__282DF8C2");
        });

        modelBuilder.Entity<Medal>(entity =>
        {
            entity.HasKey(e => e.MedalId).HasName("PK__Medal__30F05186C8CD4ADE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC2EC5A686");

            entity.Property(e => e.AccountCreationDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserMedal>(entity =>
        {
            entity.HasKey(e => e.UserMedalId).HasName("PK__UserMeda__EA2DA1A6F3BFED81");

            entity.HasOne(d => d.Medal).WithMany(p => p.UserMedals).HasConstraintName("FK_UserMedal_Medal");

            entity.HasOne(d => d.User).WithMany(p => p.UserMedals).HasConstraintName("FK_UserMedal_User");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.WorkoutPlanId).HasName("PK__WorkoutP__8C51605B28FF0CCA");

            entity.HasOne(d => d.User).WithMany(p => p.WorkoutPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__WorkoutPl__UserI__1EA48E88");

            entity.HasMany(d => d.Exercises).WithMany(p => p.WorkoutPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkoutPlanExercise",
                    r => r.HasOne<Exercise>().WithMany()
                        .HasForeignKey("ExerciseId")
                        .HasConstraintName("FK__WorkoutPl__Exerc__25518C17"),
                    l => l.HasOne<WorkoutPlan>().WithMany()
                        .HasForeignKey("WorkoutPlanId")
                        .HasConstraintName("FK__WorkoutPl__Worko__245D67DE"),
                    j =>
                    {
                        j.HasKey("WorkoutPlanId", "ExerciseId").HasName("PK__WorkoutP__66562A8BF08D4BB4");
                        j.ToTable("WorkoutPlanExercise");
                        j.IndexerProperty<int>("WorkoutPlanId").HasColumnName("WorkoutPlanID");
                        j.IndexerProperty<int>("ExerciseId").HasColumnName("ExerciseID");
                    });
        });

        modelBuilder.Entity<TokenEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Token__1788CCAC2EC5A686");

            entity.HasOne(d => d.User).WithOne(p => p.Token).HasForeignKey<TokenEntity>(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
