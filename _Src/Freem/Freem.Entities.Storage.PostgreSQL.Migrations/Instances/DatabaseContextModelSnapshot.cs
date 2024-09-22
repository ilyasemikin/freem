﻿// <auto-generated />
using System;
using Freem.Entities.Storage.PostgreSQL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    [DbContext(typeof(DatabaseContext))]
    partial class DatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("core_entities")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "core_entities", "activity_status", new[] { "active", "archived" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.ActivityEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasDefaultValue("Default")
                        .HasColumnName("name");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<int>("Status")
                        .HasColumnType("core_entities.activity_status")
                        .HasColumnName("status");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("activities_pk");

                    b.HasIndex("UserId")
                        .HasDatabaseName("activities_user_id_idx");

                    b.ToTable("activities", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.RecordEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("description");

                    b.Property<DateTimeOffset>("EndAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("end_at");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTimeOffset>("StartAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_at");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("records_pk");

                    b.HasIndex("UserId")
                        .HasDatabaseName("records_user_id_idx");

                    b.ToTable("records", "core_entities", t =>
                        {
                            t.HasCheckConstraint("records_users_check", "start_at <= end_at");
                        });
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.ActivityTagRelationEntity", b =>
                {
                    b.Property<string>("ActivityId")
                        .HasColumnType("text")
                        .HasColumnName("activity_id");

                    b.Property<string>("TagId")
                        .HasColumnType("text")
                        .HasColumnName("tag_id");

                    b.HasKey("ActivityId", "TagId")
                        .HasName("activities_tags_pk");

                    b.HasIndex("TagId")
                        .HasDatabaseName("activities_tags_tag_id_idx");

                    b.ToTable("activities_tags", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RecordActivityRelationEntity", b =>
                {
                    b.Property<string>("RecordId")
                        .HasColumnType("text")
                        .HasColumnName("record_id");

                    b.Property<string>("ActivityId")
                        .HasColumnType("text")
                        .HasColumnName("activity_id");

                    b.HasKey("RecordId", "ActivityId")
                        .HasName("records_activities_pk");

                    b.HasIndex("ActivityId")
                        .HasDatabaseName("records_activities_activity_id_idx");

                    b.ToTable("records_activities", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RecordTagRelationEntity", b =>
                {
                    b.Property<string>("RecordId")
                        .HasColumnType("text")
                        .HasColumnName("record_id");

                    b.Property<string>("TagId")
                        .HasColumnType("text")
                        .HasColumnName("tag_id");

                    b.HasKey("RecordId", "TagId")
                        .HasName("records_tags_pk");

                    b.HasIndex("TagId")
                        .HasDatabaseName("records_tags_tag_id_idx");

                    b.ToTable("records_tags", "core_entities", t =>
                        {
                            t.HasTrigger("check_records_tags_user_ids_trigger");
                        });
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RunningRecordActivityRelationEntity", b =>
                {
                    b.Property<string>("RunningRecordUserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("ActivityId")
                        .HasColumnType("text")
                        .HasColumnName("activity_id");

                    b.HasKey("RunningRecordUserId", "ActivityId")
                        .HasName("running_records_activities_pk");

                    b.HasIndex("ActivityId")
                        .HasDatabaseName("running_records_activities_activity_id_idx");

                    b.ToTable("running_records_activities", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RunningRecordTagRelationEntity", b =>
                {
                    b.Property<string>("RunningRecordUserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("TagId")
                        .HasColumnType("text")
                        .HasColumnName("tag_id");

                    b.HasKey("RunningRecordUserId", "TagId")
                        .HasName("running_records_tags_pk");

                    b.HasIndex("TagId")
                        .HasDatabaseName("running_records_tags_tag_id_idx");

                    b.ToTable("running_records_tags", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.RunningRecordEntity", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .HasMaxLength(1024)
                        .HasColumnType("character varying(1024)")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTimeOffset>("StartAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("start_at");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("UserId")
                        .HasName("running_records_pk");

                    b.ToTable("running_records", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.TagEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("tags_pk");

                    b.HasIndex("UserId")
                        .HasDatabaseName("tags_user_id_unique");

                    b.HasIndex("Name", "UserId")
                        .IsUnique()
                        .HasDatabaseName("tags_name_unique");

                    b.ToTable("tags", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.UserEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<DateTimeOffset?>("DeletedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deleted_at");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nickname");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<DateTimeOffset?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_at");

                    b.HasKey("Id")
                        .HasName("users_pk");

                    b.ToTable("users", "core_entities");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.ActivityEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("activities_users_fk");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.RecordEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("records_users_fk");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.ActivityTagRelationEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.ActivityEntity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("activities_tags_activities_fk");

                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.TagEntity", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("activities_tags_tags_fk");

                    b.Navigation("Activity");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RecordActivityRelationEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.ActivityEntity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("records_activities_activities_fk");

                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.RecordEntity", "Record")
                        .WithMany()
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("records_activities_records_fk");

                    b.Navigation("Activity");

                    b.Navigation("Record");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RecordTagRelationEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.RecordEntity", "Record")
                        .WithMany()
                        .HasForeignKey("RecordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("records_tags_records_fk");

                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.TagEntity", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("records_tags_tags_fk");

                    b.Navigation("Record");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RunningRecordActivityRelationEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.ActivityEntity", "Activity")
                        .WithMany()
                        .HasForeignKey("ActivityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("running_records_activities_activities_fk");

                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.RunningRecordEntity", "RunningRecord")
                        .WithMany()
                        .HasForeignKey("RunningRecordUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("running_records_activities_running_records_fk");

                    b.Navigation("Activity");

                    b.Navigation("RunningRecord");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.Relations.RunningRecordTagRelationEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.RunningRecordEntity", "RunningRecord")
                        .WithMany()
                        .HasForeignKey("RunningRecordUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("running_records_tags_running_records_fk");

                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.TagEntity", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("running_records_tags_tags_fk");

                    b.Navigation("RunningRecord");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.RunningRecordEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("running_records_fk");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Freem.Entities.Storage.PostgreSQL.Database.Entities.TagEntity", b =>
                {
                    b.HasOne("Freem.Entities.Storage.PostgreSQL.Database.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("tags_users_fk");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
