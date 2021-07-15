﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RepChecker.Data;

namespace RepChecker.Migrations
{
    [DbContext(typeof(ReputationDbContext))]
    [Migration("20210704075235_majgrejszynLajf")]
    partial class majgrejszynLajf
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.7");

            modelBuilder.Entity("RepChecker.DtoModels.ApplicationUserModelDto", b =>
                {
                    b.Property<string>("BattleTag")
                        .HasColumnType("TEXT");

                    b.Property<int>("Id")
                        .HasColumnType("INTEGER");

                    b.HasKey("BattleTag");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("RepChecker.DtoModels.ReputationModelDto", b =>
                {
                    b.Property<int>("ReputationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BattleTag")
                        .HasColumnType("TEXT");

                    b.Property<string>("Character")
                        .HasColumnType("TEXT");

                    b.Property<string>("FactionHref")
                        .HasColumnType("TEXT");

                    b.Property<string>("Realm")
                        .HasColumnType("TEXT");

                    b.Property<string>("ReputationName")
                        .HasColumnType("TEXT");

                    b.HasKey("ReputationId");

                    b.HasIndex("BattleTag");

                    b.ToTable("UserReputations");
                });

            modelBuilder.Entity("RepChecker.DtoModels.StandingModelDto", b =>
                {
                    b.Property<int>("StandingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CurrentValue")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Level")
                        .HasColumnType("TEXT");

                    b.Property<int>("Max")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Raw")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ReputationId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Tier")
                        .HasColumnType("INTEGER");

                    b.HasKey("StandingId");

                    b.HasIndex("ReputationId")
                        .IsUnique();

                    b.ToTable("Standings");
                });

            modelBuilder.Entity("RepChecker.DtoModels.ReputationModelDto", b =>
                {
                    b.HasOne("RepChecker.DtoModels.ApplicationUserModelDto", "ApplicationUser")
                        .WithMany("UserReputations")
                        .HasForeignKey("BattleTag");

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("RepChecker.DtoModels.StandingModelDto", b =>
                {
                    b.HasOne("RepChecker.DtoModels.ReputationModelDto", "Reputation")
                        .WithOne("Standing")
                        .HasForeignKey("RepChecker.DtoModels.StandingModelDto", "ReputationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Reputation");
                });

            modelBuilder.Entity("RepChecker.DtoModels.ApplicationUserModelDto", b =>
                {
                    b.Navigation("UserReputations");
                });

            modelBuilder.Entity("RepChecker.DtoModels.ReputationModelDto", b =>
                {
                    b.Navigation("Standing");
                });
#pragma warning restore 612, 618
        }
    }
}
