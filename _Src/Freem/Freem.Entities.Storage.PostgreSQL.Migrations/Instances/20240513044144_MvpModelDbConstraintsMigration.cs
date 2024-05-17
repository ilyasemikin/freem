﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class MvpModelDbConstraintsMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
create or replace function core_entities.check_categories_tags_user_ids()
returns trigger
language PLPGSQL
as $$
declare 
	category_user_id text;
	tag_user_id text;
begin
	select user_id from core_entities.categories where id = new.category_id into category_user_id;
	select user_id from core_entities.tags where id = new.tag_id into tag_user_id;
	
	if category_user_id <> tag_user_id then
		raise exception 'category(id = %).user_id must be equils tags(id = %).user_id',
			new.category_id,
			new.tag_id;
	end if;

	return new;
end;
$$;

create constraint trigger check_categories_tags_user_ids_trigger
	after insert or update
	on core_entities.categories_tags
	for each row
	execute procedure core_entities.check_categories_tags_user_ids();
");

			migrationBuilder.Sql(@"
create or replace function core_entities.check_records_tags_user_ids()
returns trigger
language PLPGSQL
as $$
declare
	record_user_id text;
	tag_user_id text;
begin
	select user_id from core_entities.records where id = new.record_id into record_user_id;
	select user_id from core_entities.tags where id = new.tag_id into tag_user_id;

	if record_user_id <> tag_user_id then
		raise exception 'records(id = %).user_id must be equils tags(id = %).user_id, % - %',
			new.record_id,
			new.tag_id, record_user_id, tag_user_id;
	end if;

	return new;
end;
$$;

create constraint trigger check_records_tags_user_ids_trigger
	after insert or update
	on core_entities.records_tags
	for each row
	execute procedure core_entities.check_records_tags_user_ids();
");

			migrationBuilder.Sql(@"
create or replace function core_entities.check_running_records_tags_user_ids()
returns trigger
language PLPGSQL
as $$
declare
	tag_user_id text;
begin
	select user_id from core_entities.tags where id = new.tag_id into tag_user_id;

	if new.user_id <> tag_user_id then
		raise exception 'running_records(user_id = %).user_id must be equils tags(id = %).user_id',
			new.user_id,
			new.tag_id;
	end if;

	return new;
end;
$$;

create constraint trigger check_running_records_tags_user_ids_trigger
	after insert or update
	on core_entities.running_records_tags
	for each row
	execute procedure core_entities.check_running_records_tags_user_ids();
");

			migrationBuilder.Sql(@"
create or replace function core_entities.check_records_categories_user_ids()
returns trigger
language PLPGSQL
as $$
declare
	record_user_id text;
	category_user_id text;
begin
	select user_id from core_entities.records where id = new.record_id into record_user_id;
	select user_id from core_entities.categories where id = new.category_id into category_user_id;

	if record_user_id <> category_user_id then
		raise exception 'records(id = %).user_id must be equils categories(id = %).user_id',
			new.record_id,
			new.category_id;
	end if;

	return new;
end;
$$;

create constraint trigger check_records_categories_user_ids_trigger
	after insert or update
	on core_entities.records_categories
	for each row
	execute procedure core_entities.check_records_categories_user_ids();
");

			migrationBuilder.Sql(@"
create or replace procedure core_entities.check_record_categories_count(id text)
language PLPGSQL
as $$
declare 
	categories_count int;
begin
	select count(*) from core_entities.records_categories where record_id = id into categories_count;

	if categories_count = 0 then
		raise exception 'records(id = %) must have at least one category', id;
	end if;
end;
$$;

create or replace function core_entities.check_record_categories_count_on_insert_record()
returns trigger
language PLPGSQL
as $$
begin 
	call core_entities.check_record_categories_count(new.id);
	return new;
end;
$$;

create or replace function core_entities.check_record_categories_count_on_delete_relation()
returns trigger
language PLPGSQL
as $$
begin
	call core_entities.check_record_categories_count(old.record_id);
	return old;
end;
$$;

create constraint trigger check_record_categories_count_trigger
	after insert
	on core_entities.records
	deferrable initially deferred
	for each row
	execute procedure core_entities.check_record_categories_count_on_insert_record();

create constraint trigger check_record_categories_count_trigger
	after delete
	on core_entities.records_categories
	deferrable initially deferred
	for each row
	execute procedure core_entities.check_record_categories_count_on_delete_relation();
");

			migrationBuilder.Sql(@"
create or replace function core_entities.check_running_records_categories_user_ids()
returns trigger
language PLPGSQL
as $$
declare
	category_user_id text;
begin
	select user_id from core_entities.categories where id = new.category_id into category_user_id;

	if new.user_id <> category_user_id then
		raise exception 'running_records(user_id = %).user_id must be equils categories(id = %).user_id', 
			user_id,
			new.category_id;
	end if;

	return new;
end;
$$;

create constraint trigger check_running_records_categories_user_ids_trigger
	after insert or update
	on core_entities.running_records_categories
	for each row
	execute procedure core_entities.check_running_records_categories_user_ids();
");

			migrationBuilder.Sql(@"
create or replace procedure core_entities.check_running_record_categories_count(id text)
language PLPGSQL
as $$
declare
	categories_count int;
begin
	select count(*) from core_entities.running_records_categories where user_id = id into categories_count;

	if categories_count = 0 then
		raise exception 'running_records(user_id = %) must have at least one category', id;
	end if;
end;
$$;

create or replace function core_entities.check_running_records_categories_count_on_insert_running_record()
returns trigger
language PLPGSQL
as $$
begin
	call core_entities.check_running_record_categories_count(new.user_id);
	return new;
end;
$$;

create or replace function core_entities.check_running_records_categories_count_on_delete_relation()
returns trigger
language PLPGSQL
as $$
begin
	call core_entities.check_running_record_categories_count(old.user_id);
	return old;
end;
$$;

create constraint trigger check_running_records_categories_count_trigger
	after insert
	on core_entities.running_records
	deferrable initially deferred
	for each row
	execute procedure core_entities.check_running_records_categories_count_on_insert_running_record();

create constraint trigger check_running_records_categories_count_trigger
	after delete
	on core_entities.running_records_categories
	deferrable initially deferred
	for each row
	execute procedure core_entities.check_running_records_categories_count_on_delete_relation();
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(@"
drop trigger if exists check_categories_tags_user_ids_trigger on core_entities.categories_tags;
drop function if exists core_entities.check_categories_tags_user_ids;
");

			migrationBuilder.Sql(@"
drop trigger if exists check_records_tags_user_ids_trigger on core_entities.records_tags;
drop function if exists core_entities.check_records_tags_user_ids;
");

			migrationBuilder.Sql(@"
drop trigger if exists check_running_records_tags_user_ids_trigger on core_entities.running_records_tags;
drop function if exists core_entities.check_running_records_tags_user_ids;
");

			migrationBuilder.Sql(@"
drop trigger if exists check_records_categories_user_ids_trigger on core_entities.records_categories;
drop function if exists core_entities.check_records_categories_user_ids;
");

			migrationBuilder.Sql(@"
drop trigger if exists check_record_categories_count_trigger on core_entities.records;
drop function if exists core_entities.check_record_categories_count_on_insert_record;
drop trigger if exists check_record_categories_count_trigger on core_entities.records_categories;
drop function if exists core_entities.check_record_categories_count_on_delete_relation;
drop procedure if exists core_entities.check_record_categories_count;
");

			migrationBuilder.Sql(@"
drop trigger if exists check_running_records_categories_user_ids_trigger on core_entities.running_records_categories;
drop function if exists core_entities.check_running_records_categories_user_ids;
");

			migrationBuilder.Sql(@"
drop trigger if exists check_running_records_categories_count_trigger on core_entities.running_records_categories;
drop function if exists core_entities.check_running_records_categories_count_on_delete_relation;
drop trigger if exists check_running_records_categories_count_trigger on core_entities.running_records;
drop function if exists core_entities.check_running_records_categories_count_on_insert_running_record;
drop procedure if exists core_entities.check_running_record_categories_count;
");
		}
	}
}
