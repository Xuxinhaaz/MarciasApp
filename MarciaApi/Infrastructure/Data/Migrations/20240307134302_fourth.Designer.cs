﻿// <auto-generated />
using System;
using System.Collections.Generic;
using MarciaApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MarciaApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240307134302_fourth")]
    partial class fourth
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.1.24081.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ItemProduct", b =>
                {
                    b.Property<string>("AdditionalItemsItemId")
                        .HasColumnType("text");

                    b.Property<string>("ProductsProdutId")
                        .HasColumnType("text");

                    b.HasKey("AdditionalItemsItemId", "ProductsProdutId");

                    b.HasIndex("ProductsProdutId");

                    b.ToTable("ItemProduct");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Item", b =>
                {
                    b.Property<string>("ItemId")
                        .HasColumnType("text");

                    b.Property<bool?>("IsRemoved")
                        .HasColumnType("boolean");

                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<double?>("ItemPrice")
                        .HasColumnType("double precision");

                    b.HasKey("ItemId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Location", b =>
                {
                    b.Property<string>("LocationId")
                        .HasColumnType("text");

                    b.Property<string>("AdditionalAdressInfo")
                        .HasColumnType("text");

                    b.Property<string>("CEP")
                        .HasColumnType("text");

                    b.Property<string>("Neighborhood")
                        .HasColumnType("text");

                    b.Property<string>("Number")
                        .HasColumnType("text");

                    b.Property<string>("OrderId")
                        .HasColumnType("text");

                    b.Property<string>("Street")
                        .HasColumnType("text");

                    b.HasKey("LocationId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Order", b =>
                {
                    b.Property<string>("OrderId")
                        .HasColumnType("text");

                    b.Property<bool?>("IsPaid")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("double precision");

                    b.Property<string>("UserModelId")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<string>("UserPhone")
                        .HasColumnType("text");

                    b.Property<string>("UsersId")
                        .HasColumnType("text");

                    b.HasKey("OrderId");

                    b.HasIndex("UserModelId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Product", b =>
                {
                    b.Property<string>("ProdutId")
                        .HasColumnType("text");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("text");

                    b.Property<string>("ProductName")
                        .HasColumnType("text");

                    b.Property<double?>("TotalProductPrice")
                        .HasColumnType("double precision");

                    b.HasKey("ProdutId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Size", b =>
                {
                    b.Property<string>("SizeId")
                        .HasColumnType("text");

                    b.Property<string>("SizeName")
                        .HasColumnType("text");

                    b.Property<double?>("SizePrice")
                        .HasColumnType("double precision");

                    b.HasKey("SizeId");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.UserModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<List<string>>("Roles")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("OrderProduct", b =>
                {
                    b.Property<string>("OrdersOrderId")
                        .HasColumnType("text");

                    b.Property<string>("ProductsProdutId")
                        .HasColumnType("text");

                    b.HasKey("OrdersOrderId", "ProductsProdutId");

                    b.HasIndex("ProductsProdutId");

                    b.ToTable("OrderProduct");
                });

            modelBuilder.Entity("ProductSize", b =>
                {
                    b.Property<string>("ProductsProdutId")
                        .HasColumnType("text");

                    b.Property<string>("SizesSizeId")
                        .HasColumnType("text");

                    b.HasKey("ProductsProdutId", "SizesSizeId");

                    b.HasIndex("SizesSizeId");

                    b.ToTable("ProductSize");
                });

            modelBuilder.Entity("ItemProduct", b =>
                {
                    b.HasOne("MarciaApi.Domain.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("AdditionalItemsItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarciaApi.Domain.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsProdutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Location", b =>
                {
                    b.HasOne("MarciaApi.Domain.Models.Order", null)
                        .WithOne("Location")
                        .HasForeignKey("MarciaApi.Domain.Models.Location", "OrderId");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Order", b =>
                {
                    b.HasOne("MarciaApi.Domain.Models.UserModel", null)
                        .WithMany("Orders")
                        .HasForeignKey("UserModelId");
                });

            modelBuilder.Entity("OrderProduct", b =>
                {
                    b.HasOne("MarciaApi.Domain.Models.Order", null)
                        .WithMany()
                        .HasForeignKey("OrdersOrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarciaApi.Domain.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsProdutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProductSize", b =>
                {
                    b.HasOne("MarciaApi.Domain.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductsProdutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarciaApi.Domain.Models.Size", null)
                        .WithMany()
                        .HasForeignKey("SizesSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.Order", b =>
                {
                    b.Navigation("Location");
                });

            modelBuilder.Entity("MarciaApi.Domain.Models.UserModel", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
