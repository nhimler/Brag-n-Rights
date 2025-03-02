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
            entity.HasKey(e => e.BiometricId).HasName("PK__Biometri__9CF3DB06F77DC49C");

            entity.HasOne(d => d.User).WithMany(p => p.BiometricData).HasConstraintName("FK__Biometric__UserI__6319B466");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F94A5BE33");
        });

        modelBuilder.Entity<FitnessChallenge>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("PK__FitnessC__C7AC81287FCB9A1E");

            entity.HasMany(d => d.Users).WithMany(p => p.Challenges)
                .UsingEntity<Dictionary<string, object>>(
                    "ChallengeUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Challenge__UserI__725BF7F6"),
                    l => l.HasOne<FitnessChallenge>().WithMany()
                        .HasForeignKey("ChallengeId")
                        .HasConstraintName("FK__Challenge__Chall__7167D3BD"),
                    j =>
                    {
                        j.HasKey("ChallengeId", "UserId").HasName("PK__Challeng__16D40DE2D144FF56");
                        j.ToTable("ChallengeUser");
                        j.IndexerProperty<int>("ChallengeId").HasColumnName("ChallengeID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CB93DC706A");

            entity.HasOne(d => d.Meal).WithMany(p => p.Foods)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Food__MealID__7AF13DF7");
        });

        modelBuilder.Entity<Gym>(entity =>
        {
            entity.HasKey(e => e.GymId).HasName("PK__Gym__1A3A7CB6E4A2A7E3");

            entity.HasMany(d => d.Users).WithMany(p => p.Gyms)
                .UsingEntity<Dictionary<string, object>>(
                    "GymUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__GymUser__UserID__68D28DBC"),
                    l => l.HasOne<Gym>().WithMany()
                        .HasForeignKey("GymId")
                        .HasConstraintName("FK__GymUser__GymID__67DE6983"),
                    j =>
                    {
                        j.HasKey("GymId", "UserId").HasName("PK__GymUser__CB42F07C7D83C246");
                        j.ToTable("GymUser");
                        j.IndexerProperty<int>("GymId").HasColumnName("GymID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A1E6E019A687");

            entity.HasOne(d => d.Challenge).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__Chall__6D9742D9");

            entity.HasOne(d => d.User).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__UserI__6E8B6712");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meal__ACF6A65D50DCEE7D");

            entity.HasOne(d => d.MealPlan).WithMany(p => p.Meals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Meal__MealPlanID__5A846E65");
        });

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(e => e.MealPlanId).HasName("PK__MealPlan__0620DB5612915A52");

            entity.HasOne(d => d.User).WithMany(p => p.MealPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MealPlan__UserID__56B3DD81");
        });

        modelBuilder.Entity<Medal>(entity =>
        {
            entity.HasKey(e => e.MedalId).HasName("PK__Medal__30F05186C2B3C2BC");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC324F3449");

            entity.Property(e => e.AccountCreationDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UserMedal>(entity =>
        {
            entity.HasKey(e => e.UserMedalId).HasName("PK__UserMeda__EA2DA1A6AA39744F");

            entity.HasOne(d => d.Medal).WithMany(p => p.UserMedals).HasConstraintName("FK_UserMedal_Medal");

            entity.HasOne(d => d.User).WithMany(p => p.UserMedals).HasConstraintName("FK_UserMedal_User");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.WorkoutPlanId).HasName("PK__WorkoutP__8C51605BDFE45092");

            entity.HasOne(d => d.User).WithMany(p => p.WorkoutPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__WorkoutPl__UserI__4D2A7347");

            entity.HasMany(d => d.Exercises).WithMany(p => p.WorkoutPlans)
                .UsingEntity<Dictionary<string, object>>(
                    "WorkoutPlanExercise",
                    r => r.HasOne<Exercise>().WithMany()
                        .HasForeignKey("ExerciseId")
                        .HasConstraintName("FK__WorkoutPl__Exerc__53D770D6"),
                    l => l.HasOne<WorkoutPlan>().WithMany()
                        .HasForeignKey("WorkoutPlanId")
                        .HasConstraintName("FK__WorkoutPl__Worko__52E34C9D"),
                    j =>
                    {
                        j.HasKey("WorkoutPlanId", "ExerciseId").HasName("PK__WorkoutP__66562A8B13864FC1");
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
