# SpiralTechExam

Architecture:
- Bank.Domain: Contains all the domain classes.
- Bank.AppLogic: Is the Business logic layer of the application
- Bank.Data: Is used to access data. This usses Dapper.
- Bank.DbUp: .net core console application. Creates the database tables using DbUp.
- Bank.Api: Is a .net core api application for exposing services.
- Bank.Web: Is a .net core MVC application for the presentation.

Unit Test:
- Bank.AppLogic.Test

How To Setup
- Make sure you have the following installed
 - Visual Studio 2017
 - MS SQL Server with SQLExpress instance
- Open BankSystem.sln using Visual Studio 2017
- Build the solution
- Run Bank.DbUp to create the database and tables.
- Run Bank.Api
- Run Bank.Web
