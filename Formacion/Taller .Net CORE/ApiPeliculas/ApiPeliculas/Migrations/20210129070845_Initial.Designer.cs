﻿// <auto-generated />
using System;
using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ApiPeliculas.Migrations
{
    [DbContext(typeof(EntityContext))]
    [Migration("20210129070845_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Actor", b =>
                {
                    b.Property<Guid>("Person_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Film_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Film_ID1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Person_ID");

                    b.HasIndex("Film_ID1");

                    b.ToTable("Actor");
                });

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Director", b =>
                {
                    b.Property<Guid>("Person_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Film_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Person_ID");

                    b.ToTable("Director");
                });

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Film", b =>
                {
                    b.Property<Guid>("Film_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("DirectorPerson_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("MinuteRunTime")
                        .HasColumnType("int");

                    b.Property<string>("Released")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Film_ID");

                    b.HasIndex("DirectorPerson_ID");

                    b.ToTable("Film");
                });

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Rating", b =>
                {
                    b.Property<Guid>("Rating_ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Film_ID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("Film_ID1")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Rating_ID");

                    b.HasIndex("Film_ID1");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Actor", b =>
                {
                    b.HasOne("ApiPeliculas.Models.Entities.Film", null)
                        .WithMany("Actors")
                        .HasForeignKey("Film_ID1");
                });

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Film", b =>
                {
                    b.HasOne("ApiPeliculas.Models.Entities.Director", "Director")
                        .WithMany()
                        .HasForeignKey("DirectorPerson_ID");

                    b.Navigation("Director");
                });

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Rating", b =>
                {
                    b.HasOne("ApiPeliculas.Models.Entities.Film", null)
                        .WithMany("Ratings")
                        .HasForeignKey("Film_ID1");
                });

            modelBuilder.Entity("ApiPeliculas.Models.Entities.Film", b =>
                {
                    b.Navigation("Actors");

                    b.Navigation("Ratings");
                });
#pragma warning restore 612, 618
        }
    }
}
