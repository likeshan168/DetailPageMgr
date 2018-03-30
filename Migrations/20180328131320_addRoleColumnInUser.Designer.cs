﻿// <auto-generated />
using DetailPage.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DetailPage.Migrations
{
    [DbContext(typeof(DetailPageContext))]
    [Migration("20180328131320_addRoleColumnInUser")]
    partial class addRoleColumnInUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DetailPage.Models.DetailPageModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HtmlContent");

                    b.Property<string>("Name")
                        .HasMaxLength(30);

                    b.Property<string>("ProductNo")
                        .HasMaxLength(30);

                    b.Property<string>("Remark")
                        .HasMaxLength(400);

                    b.Property<string>("Url")
                        .HasMaxLength(1000);

                    b.HasKey("ID");

                    b.ToTable("DetailPage");
                });

            modelBuilder.Entity("DetailPage.Models.UserModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .HasMaxLength(30);

                    b.Property<string>("Role")
                        .HasMaxLength(30);

                    b.Property<string>("UserName")
                        .HasMaxLength(30);

                    b.HasKey("ID");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
