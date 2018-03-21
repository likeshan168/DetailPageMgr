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
    [Migration("20180320083705_addHtmlContentColumn")]
    partial class addHtmlContentColumn
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

                    b.Property<string>("Name");

                    b.Property<string>("Remark");

                    b.Property<string>("Url");

                    b.HasKey("ID");

                    b.ToTable("DetailPage");
                });
#pragma warning restore 612, 618
        }
    }
}
