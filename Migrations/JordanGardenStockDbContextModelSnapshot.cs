﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JordanGardenStockWebAPI.Migrations
{
    [DbContext(typeof(JordanGardenStockDbContext))]
    partial class JordanGardenStockDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("varchar(200)")
                        .HasColumnOrder(4);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnOrder(3);

                    b.Property<string>("Mail")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnOrder(2);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CompanyId")
                        .HasColumnType("int")
                        .HasColumnName("Company_Id")
                        .HasColumnOrder(1);

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(6,3)")
                        .HasColumnOrder(3);

                    b.Property<int?>("Quantity")
                        .HasColumnType("int")
                        .HasColumnOrder(5);

                    b.Property<string>("Size")
                        .HasColumnType("varchar(20)")
                        .HasColumnOrder(4);

                    b.Property<int>("TillandsiaId")
                        .HasColumnType("int")
                        .HasColumnName("Tillandsia_Id")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("TillandsiaId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Tillandsia", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .HasColumnType("varchar(300)")
                        .HasColumnName("Image")
                        .HasColumnOrder(3);

                    b.Property<string>("NameChi")
                        .HasColumnType("varchar(20)")
                        .HasColumnName("Name_Chi")
                        .HasColumnOrder(2);

                    b.Property<string>("NameEng")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasColumnName("Name_Eng")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Tillandsias");
                });

            modelBuilder.Entity("Product", b =>
                {
                    b.HasOne("Company", "Company")
                        .WithMany("Products")
                        .HasForeignKey("CompanyId")
                        .IsRequired()
                        .HasConstraintName("ForeignKey_CompanyId");

                    b.HasOne("Tillandsia", "Tillandsia")
                        .WithMany("Products")
                        .HasForeignKey("TillandsiaId")
                        .IsRequired()
                        .HasConstraintName("ForeignKey_TillandsiaId");

                    b.Navigation("Company");

                    b.Navigation("Tillandsia");
                });

            modelBuilder.Entity("Company", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Tillandsia", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
