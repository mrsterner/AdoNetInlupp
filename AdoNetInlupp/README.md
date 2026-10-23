## AdoNetInlupp

A small console application for managing a movie register. Built with C# and ADO.NET against a SQL Server database.

## Setup

Start the application:

dotnet run

On first start the application creates the MovieRegistry database, the Genre and Movie tables, and seed it.

## SQL
The queries used are present in ``Sql/01-create-database.sql`` and ``Sql/02-create-tables-and-seed.sql``
but the program will create and seed on first start anyway.

## Features
- List all movies with their genre
- List movies by genre
- Add a movie and link it to an existing genre
- Delete a movie