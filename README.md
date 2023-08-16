# XtraWork

XtraWork Web API. A Restful Web API that registers employees' activities.


## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.


### Prerequisites

Operation System `Linux`, `Windows` or `macOS`. Make sure you have [.NET 7.0](https://dotnet.microsoft.com/download) and [Docker](https://docs.docker.com/engine/install/)  installed globally on your machine.


### Installing

Clone the project into your machine. Go to your terminal and cd into `XtraWork.API` directory. Run `docker compose up --build` to create and start API and Database containers. Navigate to `http://localhost:4221/swagger/index.html`.


## Deployment

The app can be published to a web server or a cloud server with minimum configuration.


## Running the tests

### Unit tests

Go to your terminal and cd into `XtraWork.UnitTests` directory. Run `dotnet test` to execute the unit tests.

### Integration tests

Go to your terminal and cd into `XtraWork.IntegrationTests` directory. Run `dotnet test` to execute the integration tests.


## Built With

* [.NET 7.0](https://dotnet.microsoft.com/) - The back-end framework used to create the API
* [SQL Sever 2022](https://www.microsoft.com/pt-br/sql-server/sql-server-2022) - The database used to persist data
* [Docker](https://www.docker.com/) - The platform used to build and run the application in containers
* [Scrutor](https://github.com/khellang/Scrutor) - The library used to register decorator classes in service container
* [Serilog](https://serilog.net/) - The library used for logging 
* [Bogus](https://github.com/bchavez/Bogus) - The library used to generate random fake data for testing purposes
* [Xunit](https://xunit.net/) - The testing framework used to create unit and integration tests
* [Moq](https://moq.github.io/moq/) - The mocking framework used to mock data in unit tests
* [Testcontainers](https://testcontainers.com/) - The framework used to run database instances in docker containers for integration tests
* [Respawn](https://github.com/jbogard/Respawn) - The database cleaner used in integration tests
* [FluentAssertions](https://fluentassertions.com/) - The library used to make test assertions more readable


## Acknowledgments

This project is based on the following content:
* [Building Web API using ASP.NET](https://juldhais.net/building-web-api-using-asp-net-core-for-dummies-3e0c59881432)
* [Implementing In-Memory Caching using Decorator Pattern](https://juldhais.net/implementing-in-memory-caching-using-decorator-pattern-in-asp-net-core-10f84dcae70b)
* [REST API Tutorial - Full Course](https://www.youtube.com/playlist?list=PLUOequmGnXxOgmSDWU7Tl6iQTsOtyjtwU)
