﻿// <auto-generated />
using System;
using JamTeamFormingTool.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JamTeamFormingTool.Migrations
{
    [DbContext(typeof(MyDBContext))]
    [Migration("20230323083508_Initial-Create")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JamTeamFormingTool.Models.CoverageSet", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("RoleTemplateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("RoleTemplateName");

                    b.ToTable("CoverageSets");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.CoverageSetRoleCategory", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("CoverageSetID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CoverageSetID");

                    b.ToTable("CoverageSetRoleCategories");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.JamTeamFormingSession", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AdminEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AdminPassCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("GenericPassCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Info")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Phase")
                        .HasColumnType("int");

                    b.Property<string>("RoleTemplateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("RoleTemplateName");

                    b.ToTable("JamTeamFormingSessions");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.Participant", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("PassCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Portfolio")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int?>("Region")
                        .HasColumnType("int");

                    b.Property<int>("SessionID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("SessionID");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.Role", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Description")
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("RoleTemplateName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("RoleTemplateName");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.RoleTemplate", b =>
                {
                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("AuthorizePassCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsStaged")
                        .HasColumnType("bit");

                    b.HasKey("Name");

                    b.ToTable("RoleTemplates");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.Team", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Handle")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("Name")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PassCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Pitch")
                        .HasMaxLength(280)
                        .HasColumnType("nvarchar(280)");

                    b.Property<int?>("Region")
                        .HasColumnType("int");

                    b.Property<int>("SessionID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("SessionID");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("ParticipantRole", b =>
                {
                    b.Property<int>("ParticipantsID")
                        .HasColumnType("int");

                    b.Property<int>("RolesID")
                        .HasColumnType("int");

                    b.HasKey("ParticipantsID", "RolesID");

                    b.HasIndex("RolesID");

                    b.ToTable("ParticipantRole");
                });

            modelBuilder.Entity("RoleCategoryRole", b =>
                {
                    b.Property<int>("RoleCategoriesID")
                        .HasColumnType("int");

                    b.Property<int>("RolesID")
                        .HasColumnType("int");

                    b.HasKey("RoleCategoriesID", "RolesID");

                    b.HasIndex("RolesID");

                    b.ToTable("RoleCategoryRole");
                });

            modelBuilder.Entity("TeamRole", b =>
                {
                    b.Property<int>("OpenRolesID")
                        .HasColumnType("int");

                    b.Property<int>("TeamsID")
                        .HasColumnType("int");

                    b.HasKey("OpenRolesID", "TeamsID");

                    b.HasIndex("TeamsID");

                    b.ToTable("TeamRole");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.CoverageSet", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.RoleTemplate", "RoleTemplate")
                        .WithMany("CoverageSets")
                        .HasForeignKey("RoleTemplateName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleTemplate");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.CoverageSetRoleCategory", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.CoverageSet", "CoverageSet")
                        .WithMany("RoleCategories")
                        .HasForeignKey("CoverageSetID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CoverageSet");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.JamTeamFormingSession", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.RoleTemplate", "RoleTemplate")
                        .WithMany()
                        .HasForeignKey("RoleTemplateName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleTemplate");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.Participant", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.JamTeamFormingSession", "Session")
                        .WithMany()
                        .HasForeignKey("SessionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.Role", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.RoleTemplate", "RoleTemplate")
                        .WithMany("Roles")
                        .HasForeignKey("RoleTemplateName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RoleTemplate");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.Team", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.JamTeamFormingSession", "Session")
                        .WithMany()
                        .HasForeignKey("SessionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("ParticipantRole", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.Participant", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JamTeamFormingTool.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RoleCategoryRole", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.CoverageSetRoleCategory", null)
                        .WithMany()
                        .HasForeignKey("RoleCategoriesID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JamTeamFormingTool.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TeamRole", b =>
                {
                    b.HasOne("JamTeamFormingTool.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("OpenRolesID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JamTeamFormingTool.Models.Team", null)
                        .WithMany()
                        .HasForeignKey("TeamsID")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.CoverageSet", b =>
                {
                    b.Navigation("RoleCategories");
                });

            modelBuilder.Entity("JamTeamFormingTool.Models.RoleTemplate", b =>
                {
                    b.Navigation("CoverageSets");

                    b.Navigation("Roles");
                });
#pragma warning restore 612, 618
        }
    }
}
