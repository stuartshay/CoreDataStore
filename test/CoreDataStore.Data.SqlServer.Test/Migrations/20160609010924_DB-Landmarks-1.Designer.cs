﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CoreDataStore.Data.SqlServer;

namespace CoreDataStore.Data.SqlServer.Test.Migrations
{
    [DbContext(typeof(NYCLandmarkContext))]
    [Migration("20160609010924_DB-Landmarks-1")]
    partial class DBLandmarks1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rc2-20901")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreDataStore.Domain.Entities.Landmark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("BBL");

                    b.Property<long>("BIN_NUMBER");

                    b.Property<int>("BLOCK");

                    b.Property<string>("BOUNDARIES");

                    b.Property<string>("BoroughID");

                    b.Property<string>("CALEN_DATE");

                    b.Property<bool>("COUNT_BLDG");

                    b.Property<string>("DESIG_ADDR");

                    b.Property<string>("DESIG_DATE");

                    b.Property<string>("HIST_DISTR");

                    b.Property<string>("LAST_ACTIO");

                    b.Property<string>("LM_NAME");

                    b.Property<string>("LM_TYPE");

                    b.Property<int>("LOT");

                    b.Property<string>("LP_NUMBER");

                    b.Property<bool>("MOST_CURRE");

                    b.Property<string>("NON_BLDG");

                    b.Property<string>("OTHER_HEAR");

                    b.Property<string>("PLUTO_ADDR");

                    b.Property<string>("PUBLIC_HEA");

                    b.Property<bool>("SECND_BLDG");

                    b.Property<string>("STATUS");

                    b.Property<string>("STATUS_NOT");

                    b.Property<bool>("VACANT_LOT");

                    b.HasKey("Id");

                    b.ToTable("Landmark");
                });

            modelBuilder.Entity("CoreDataStore.Domain.Entities.LPCReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Architect")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("Borough")
                        .HasAnnotation("MaxLength", 20);

                    b.Property<DateTime>("DateDesignated");

                    b.Property<string>("LPCId");

                    b.Property<string>("LPNumber");

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 200);

                    b.Property<string>("ObjectType")
                        .HasAnnotation("MaxLength", 50);

                    b.Property<bool>("PhotoStatus");

                    b.Property<string>("PhotoURL");

                    b.Property<string>("Street");

                    b.Property<string>("Style");

                    b.HasKey("Id");

                    b.ToTable("LPCReport");
                });
        }
    }
}