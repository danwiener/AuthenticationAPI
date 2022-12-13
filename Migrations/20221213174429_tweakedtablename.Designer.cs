﻿// <auto-generated />
using System;
using Authentication.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Authentication.Migrations
{
    [DbContext(typeof(FFContextDb))]
    [Migration("20221213174429_tweakedtablename")]
    partial class tweakedtablename
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Authentication.Models.League", b =>
                {
                    b.Property<int>("LeagueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "LeagueId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeagueId"));

                    b.Property<int>("Creator")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "creator");

                    b.Property<string>("LeagueName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "leaguename");

                    b.Property<int>("MaxTeams")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "maxteams");

                    b.HasKey("LeagueId");

                    b.HasIndex("LeagueName")
                        .IsUnique();

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("Authentication.Models.LeagueRules", b =>
                {
                    b.Property<int>("LeagueRulesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "leaguerulesid");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LeagueRulesId"));

                    b.Property<int>("DCount")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "defensecount");

                    b.Property<int>("FgFiftyToSixty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgfiftytosixty");

                    b.Property<int>("FgFortyToFifty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgfortytofifty");

                    b.Property<int>("FgMissedFifty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgmissedfifty");

                    b.Property<int>("FgMissedSixty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgmissedsixty");

                    b.Property<int>("FgMissedTen")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgmissedten");

                    b.Property<int>("FgMissedThirty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgmissedthirty");

                    b.Property<int>("FgMissedTwenty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgmissedtwenty");

                    b.Property<int>("FgMissedforty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgmissedforty");

                    b.Property<int>("FgPuntBlock")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgpuntblock");

                    b.Property<int>("FgSixtyPlus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgsixtyplus");

                    b.Property<int>("FgTenToTwenty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgtentotwenty");

                    b.Property<int>("FgThirtyToForty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgthirtytoforty");

                    b.Property<int>("FgTwentyToThirty")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fgtwentytothirty");

                    b.Property<int>("FiveHundredYardPassBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fivehundredyardpassbonus");

                    b.Property<int>("FortyYardPassBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fortyyardpassbonus");

                    b.Property<int>("FortyYardRushReceivingBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fortyyardrushreceivingbonus");

                    b.Property<int>("FumbleDefense")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fumbledefense");

                    b.Property<int>("FumbleOffense")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fumbleoffense");

                    b.Property<int>("FumbleTd")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "fumbletd");

                    b.Property<int>("IntTd")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "inttd");

                    b.Property<int>("InterceptionDefense")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "interceptiondefense");

                    b.Property<int>("InterceptionOffense")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "interceptionoffense");

                    b.Property<int>("KCount")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "kcount");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "leagueid");

                    b.Property<int>("MaxPlayers")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "maxplayers");

                    b.Property<int>("MaxTeams")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "maxteams");

                    b.Property<int>("OneHundredYardRushReceivingBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "onehundredyardrushreceivingbonus");

                    b.Property<double>("PPC")
                        .HasColumnType("float")
                        .HasAnnotation("Relational:JsonPropertyName", "ppc");

                    b.Property<double>("PPI")
                        .HasColumnType("float")
                        .HasAnnotation("Relational:JsonPropertyName", "ppi");

                    b.Property<double>("PPR")
                        .HasColumnType("float")
                        .HasAnnotation("Relational:JsonPropertyName", "ppr");

                    b.Property<int>("PPTenRush")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "pptenrush");

                    b.Property<int>("PPTwentyFiveYdsPass")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "pptwentyfivepass");

                    b.Property<int>("PassingTDPoints")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "passingtdpoints");

                    b.Property<int>("QbCount")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "qbcount");

                    b.Property<int>("RbCount")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "rbcount");

                    b.Property<int>("ReceivingTDPoints")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "receivingtdpoints");

                    b.Property<int>("ReturnTd")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "returntd");

                    b.Property<int>("RushingTDPoints")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "rushingtdpoints");

                    b.Property<int>("SackDefense")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "sackdefense");

                    b.Property<int>("SafetyDefense")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "safetydefense");

                    b.Property<int>("SafetyOffense")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "safetyoffense");

                    b.Property<int>("SixtyYardPassBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "sixtyyardpassbonus");

                    b.Property<int>("SixtyYardRushReceivingBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "sixtyyardrushreceivingbonus");

                    b.Property<double>("TackleDefense")
                        .HasColumnType("float")
                        .HasAnnotation("Relational:JsonPropertyName", "tackledefense");

                    b.Property<int>("TeCount")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "tecount");

                    b.Property<int>("ThreeHundredYardPassBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "threehundredyardpassbonus");

                    b.Property<int>("TwoHundredYardRushReceivingBonus")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "twohundredyardrushreceivingbonus");

                    b.Property<int>("TwoPointConversion")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "twopointconversion");

                    b.Property<int>("WrCount")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "wrcount");

                    b.Property<int>("XpMade")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "xpmade");

                    b.Property<int>("XpMissed")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "xpmissed");

                    b.HasKey("LeagueRulesId");

                    b.ToTable("LeagueRules");
                });

            modelBuilder.Entity("Authentication.Models.League_Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.HasIndex("TeamId")
                        .IsUnique();

                    b.ToTable("LeagueTeams");
                });

            modelBuilder.Entity("Authentication.Models.ResetToken", b =>
                {
                    b.Property<string>("Email")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "email");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "token");

                    b.HasKey("Email");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.ToTable("ResetTokens");
                });

            modelBuilder.Entity("Authentication.Models.Team", b =>
                {
                    b.Property<int>("TeamId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "UserId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TeamId"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<int>("LeagueId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "LeagueId");

                    b.Property<string>("TeamName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)")
                        .HasAnnotation("Relational:JsonPropertyName", "teamname");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "UserId");

                    b.HasKey("TeamId");

                    b.HasIndex("TeamName")
                        .IsUnique();

                    b.ToTable("Team");
                });

            modelBuilder.Entity("Authentication.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("Relational:JsonPropertyName", "UserId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "user_name");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Authentication.Models.UserToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Authentication.Models.User_League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LeagueId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LeagueId");

                    b.HasIndex("UserId");

                    b.ToTable("UserLeagues");
                });

            modelBuilder.Entity("Authentication.Models.League_Team", b =>
                {
                    b.HasOne("Authentication.Models.League", "League")
                        .WithMany("League_Teams")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Authentication.Models.Team", "Team")
                        .WithOne("League_Team")
                        .HasForeignKey("Authentication.Models.League_Team", "TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Authentication.Models.User_League", b =>
                {
                    b.HasOne("Authentication.Models.League", "League")
                        .WithMany("User_Leagues")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Authentication.Models.User", "User")
                        .WithMany("User_Leagues")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("League");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Authentication.Models.League", b =>
                {
                    b.Navigation("League_Teams");

                    b.Navigation("User_Leagues");
                });

            modelBuilder.Entity("Authentication.Models.Team", b =>
                {
                    b.Navigation("League_Team")
                        .IsRequired();
                });

            modelBuilder.Entity("Authentication.Models.User", b =>
                {
                    b.Navigation("User_Leagues");
                });
#pragma warning restore 612, 618
        }
    }
}
