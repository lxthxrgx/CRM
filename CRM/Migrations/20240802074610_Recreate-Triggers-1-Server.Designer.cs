﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SRMAgreement.Data_Base;

#nullable disable

namespace SRMAgreement.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20240802074610_Recreate-Triggers-1-Server")]
    partial class RecreateTriggers1Server
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("SRMAgreement.Class.Status", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("status");
                });

            modelBuilder.Entity("SRMAgreement.Class._1D", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BanckAccount")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Director")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResPerson")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Tov")
                        .HasColumnType("INTEGER");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("edryofop_Data")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("rnokpp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("D1");
                });

            modelBuilder.Entity("SRMAgreement.Class._2D", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("NameGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberGroup")
                        .HasColumnType("INTEGER");

                    b.Property<string>("PIBS")
                        .HasColumnType("TEXT");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("area")
                        .HasColumnType("REAL");

                    b.Property<string>("rent")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("D2");
                });

            modelBuilder.Entity("SRMAgreement.Class._3D", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AktDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("DogovirOrendu")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberGroup")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Prumitka")
                        .HasColumnType("TEXT");

                    b.Property<string>("Stan")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StrokDii")
                        .HasColumnType("TEXT");

                    b.Property<string>("Suma")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("D3");
                });

            modelBuilder.Entity("SRMAgreement.Class._4D", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("AktDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("DogovirSuborendu")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Less_than_year")
                        .HasColumnType("TEXT");

                    b.Property<string>("NameGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberGroup")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Prumitka")
                        .HasColumnType("TEXT");

                    b.Property<string>("Stan")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StrokDii")
                        .HasColumnType("TEXT");

                    b.Property<string>("Suma")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("D4");
                });

            modelBuilder.Entity("SRMAgreement.Class._5D", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("NumDog")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("NumberGroup")
                        .HasColumnType("INTEGER");

                    b.Property<string>("OhronnaComp")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PathToFile")
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResPerson")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StrokDii")
                        .HasColumnType("TEXT");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("D5");
                });

            modelBuilder.Entity("SRMAgreement.Class._6D", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BanckAccount")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameGroup")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NameTov")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasColumnType("TEXT");

                    b.Property<string>("ResPerson")
                        .HasColumnType("TEXT");

                    b.Property<string>("UnifiedStateRegister")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("director")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("D6");
                });
#pragma warning restore 612, 618
        }
    }
}
