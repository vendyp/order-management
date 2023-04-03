# Order Management System

This is a simple project of order management system and build using latest version of .NET

# Prerequisite

1. Install [.NET][download_dotnet] latest version (Windows, Linux supported)
2. Install SQL Server Express or Developer
3. Install dotnet ef tools using command "dotnet tool install --global dotnet-ef", if already installed use this to update to latest version available "dotnet tool update --global dotnet-ef"

# Actors

This repository order management system is built using RBAC (Role-Based Access Control), for full understanding you can go [here][rbac].

It have 3 roles :

1. Administrator
2. Owner
3. Staff

## Role Administrator

This role can only access user management only.

## Owner

This role can access dashboard overall data order, and so on, but can not create order.

## Staff

This role can access order, product, mostly data entry administrator.

# Features

## User

TBD

## Product

TBD

### Stock

TBD

## Order

TBD

## Dashboard

TBD

## Report

TBD

# ERD (Entity Relation Diagram)

TBD

# Docker Support

TBD

[download_dotnet]: https://dotnet.microsoft.com/en-us/download/dotnet/7.0
[rbac]: https://en.wikipedia.org/wiki/Role-based_access_control