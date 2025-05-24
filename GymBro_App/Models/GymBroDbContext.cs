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

    public virtual DbSet<FitnessChallenge> FitnessChallenges { get; set; }

    public virtual DbSet<Food> Foods { get; set; }

    public virtual DbSet<GymUser> GymUsers { get; set; }

    public virtual DbSet<Leaderboard> Leaderboards { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<MealPlan> MealPlans { get; set; }

    public virtual DbSet<Medal> Medals { get; set; }

    public virtual DbSet<TokenEntity> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserMedal> UserMedals { get; set; }

    public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }

    public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }

    public virtual DbSet<WorkoutPlanTemplate> WorkoutPlanTemplates { get; set; }
    public virtual DbSet<WorkoutPlanTemplateExercise> WorkoutPlanTemplateExercises { get; set; }
    
    
    public virtual DbSet<StepCompetition> StepCompetitions { get; set; }  
    public virtual DbSet<StepCompetitionParticipant> StepCompetitionParticipants { get; set; }
    public virtual DbSet<StepCompetition> StepCompetition { get; set; }


    public virtual DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Only configure SQL Server if nobody else (e.g. your tests) has already done so:
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Name=GymBroAzureConnection");
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BiometricDatum>(entity =>
        {
            entity.HasKey(e => e.BiometricId).HasName("PK__Biometri__9CF3DB06B4B7D2A9");

            entity.HasOne(d => d.User).WithMany(p => p.BiometricData).HasConstraintName("FK__Biometric__UserI__3A179ED3");
        });

        modelBuilder.Entity<FitnessChallenge>(entity =>
        {
            entity.HasKey(e => e.ChallengeId).HasName("PK__FitnessC__C7AC8128C57F8E1E");

            entity.HasMany(d => d.Users).WithMany(p => p.Challenges)
                .UsingEntity<Dictionary<string, object>>(
                    "ChallengeUser",
                    r => r.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK__Challenge__UserI__4959E263"),
                    l => l.HasOne<FitnessChallenge>().WithMany()
                        .HasForeignKey("ChallengeId")
                        .HasConstraintName("FK__Challenge__Chall__4865BE2A"),
                    j =>
                    {
                        j.HasKey("ChallengeId", "UserId").HasName("PK__Challeng__16D40DE2B0E6C01A");
                        j.ToTable("ChallengeUser");
                        j.IndexerProperty<int>("ChallengeId").HasColumnName("ChallengeID");
                        j.IndexerProperty<int>("UserId").HasColumnName("UserID");
                    });
        });

        // Map singular class names to singular tables
        modelBuilder.Entity<WorkoutPlanTemplate>()
            .ToTable("WorkoutPlanTemplate");
        modelBuilder.Entity<WorkoutPlanTemplateExercise>()
            .ToTable("WorkoutPlanTemplateExercise");

        modelBuilder.Entity<Food>(entity =>
        {
            entity.HasKey(e => e.FoodId).HasName("PK__Food__856DB3CB9C45FC45");

            entity.HasOne(d => d.Meal).WithMany(p => p.Foods)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Food__MealID__54CB950F");
        });

        modelBuilder.Entity<GymUser>(entity =>
        {
            entity.HasKey(e => e.GymUserId).HasName("PK__GymUser__198C43EE844A57A4");

            entity.HasOne(d => d.User).WithMany(p => p.GymUsers).HasConstraintName("FK__GymUser__UserID__473C8FC7");
        });

        modelBuilder.Entity<Leaderboard>(entity =>
        {
            entity.HasKey(e => e.LeaderboardId).HasName("PK__Leaderbo__B358A1E6E07DC5EC");

            entity.HasOne(d => d.Challenge).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__Chall__44952D46");

            entity.HasOne(d => d.User).WithMany(p => p.Leaderboards)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Leaderboa__UserI__4589517F");
        });

        modelBuilder.Entity<Meal>(entity =>
        {
            entity.HasKey(e => e.MealId).HasName("PK__Meal__ACF6A65DED0713C7");

            entity.HasOne(d => d.MealPlan).WithMany(p => p.Meals)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Meal__MealPlanID__318258D2");
        });

        modelBuilder.Entity<MealPlan>(entity =>
        {
            entity.HasKey(e => e.MealPlanId).HasName("PK__MealPlan__0620DB56BBCE7C0C");

            entity.HasOne(d => d.User).WithMany(p => p.MealPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__MealPlan__UserID__2DB1C7EE");
        });

        modelBuilder.Entity<Medal>(entity =>
        {
            entity.HasKey(e => e.MedalId).HasName("PK__Medal__30F05186EFBE82FF");
        });

        modelBuilder.Entity<TokenEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Token__1788CC4C526EEF32");

            entity.Property(e => e.UserId).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithOne(p => p.Token).HasConstraintName("FK_Token_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCAC718E8A76");

            entity.Property(e => e.AccountCreationDate).HasDefaultValueSql("(getdate())");
            entity.HasAlternateKey(u => u.IdentityUserId);
        });

        modelBuilder.Entity<UserMedal>(entity =>
        {
            entity.HasKey(e => e.UserMedalId).HasName("PK__UserMeda__EA2DA1A666D27E82");

            entity.HasOne(d => d.Medal).WithMany(p => p.UserMedals).HasConstraintName("FK_UserMedal_Medal");

            entity.HasOne(d => d.User).WithMany(p => p.UserMedals).HasConstraintName("FK_UserMedal_User");
        });


        modelBuilder.Entity<StepCompetition>(entity =>
        {
            entity.HasKey(e => e.CompetitionID);

            entity.HasOne(e => e.Creator)
                .WithMany(u => u.CreatedCompetitions)
                .HasPrincipalKey(u => u.IdentityUserId)
                .HasForeignKey(e => e.CreatorIdentityId)
                .OnDelete(DeleteBehavior.Cascade); // Optional: adjust based on your needs
        });



        modelBuilder.Entity<WorkoutExercise>(entity =>
        {
            entity.HasKey(e => e.WorkoutExercisesId).HasName("PK__WorkoutE__B81DE8C4DF9C2426");

            entity.Property(e => e.WorkoutExercisesId).ValueGeneratedNever();

            entity.HasOne(d => d.WorkoutPlanExercise).WithMany(p => p.WorkoutExercises).HasConstraintName("FK__WorkoutEx__Worko__2116E6DF");

            entity.HasOne(d => d.WorkoutPlan).WithMany(p => p.WorkoutExercises)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__WorkoutEx__Worko__2022C2A6");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.WorkoutPlanId).HasName("PK__WorkoutP__8C51605B9B7D06CA");

            entity.HasOne(d => d.User).WithMany(p => p.WorkoutPlans)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__WorkoutPl__UserI__1975C517");
        });


            // Store the DifficultyLevel enum as its string value in the DB
            modelBuilder.Entity<WorkoutPlanTemplate>()
                        .Property(e => e.DifficultyLevel)
                        .HasConversion<string>();

            // Configure the 1-to-many relationship
            modelBuilder.Entity<WorkoutPlanTemplate>()
                        .HasMany(t => t.Exercises)
                        .WithOne(x => x.Template)
                        .HasForeignKey(x => x.WorkoutPlanTemplateID)
                        .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<StepCompetitionParticipant>()
            .HasOne(p => p.StepCompetition)
            .WithMany(c => c.Participants)
            .HasForeignKey(p => p.StepCompetitionId)
            .OnDelete(DeleteBehavior.Cascade);  // Cascade only here

        // Participant → User
        modelBuilder.Entity<StepCompetitionParticipant>()
            .HasOne(p => p.User)
            .WithMany(u => u.StepCompetitions)
            .HasForeignKey(p => p.IdentityId)
            .HasPrincipalKey(u => u.IdentityUserId)
            .OnDelete(DeleteBehavior.Restrict);  // Prevent multiple cascade paths

        modelBuilder.Entity<TokenEntity>(entity =>

        modelBuilder.Entity<WorkoutPlanExercise>(entity =>
        {
            entity.HasKey(e => e.WorkoutPlanExerciseId).HasName("PK__WorkoutP__8D1477A67A1078DF");

            entity.HasOne(d => d.WorkoutPlan).WithMany(p => p.WorkoutPlanExercises)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__WorkoutPl__Worko__1C5231C2");
        }));

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
