# Mool
Mool is a relatively simple example solution on how to create a message board web application.  
I made this solution to learn new stuff. I've always been a very backend focused developer, which  
made me feel like i was missing out on other fundamentals of web development.  
I therefore created this solution.  

Feel free to look around and ~~steal stuff~~ get inspired 😊.  

## Purpose of this readme
The remainder of this readme lays out what the solution consists of and how to get it up and running.  
I have not gone into a lot of detail, if you want that, you will have to read the code 😛.

## Technologies
### Backend
* [ASP.NET Core 5](https://github.com/dotnet/aspnetcore) - The API framework.
* [MongoDB](https://github.com/mongodb/mongo) - The database.
* Microsoft.AspNetCore.Authentication/Authorization - For JWT based identity handling.
* [MediatR](https://github.com/jbogard/MediatR) - To handle CQRS and validation pipeline.
* [FluentValidation](https://github.com/FluentValidation/FluentValidation) - To validate requests before they reach handlers.
* [NSwag](https://github.com/RicoSuter/NSwag) - To generate frontend client on build.
* [Autofixture](https://github.com/AutoFixture/AutoFixture) - To generate objects for tests.
* [Moq](https://github.com/moq/moq) - Mocking framework for tests.
* [xUnit.net](https://github.com/xunit/xunit) - Test framework.
* [CliFx](https://github.com/Tyrrrz/CliFx) - Commandline framework for the DeveloperOperations project.

### Frontend
* [React](https://github.com/facebook/react) - Frontend framework.
* [Redux](https://github.com/reduxjs/redux) - For managing state.
* [Styled Components](https://github.com/styled-components/styled-components) - For styling components.
* [Formik](https://github.com/jaredpalmer/formik) - To handle forms.
* [Jest](https://github.com/facebook/jest) - Test framework.
* [React testing library](https://github.com/testing-library/react-testing-library) - Testing utilities.
* [Cypress](https://github.com/cypress-io/cypress) - E2E test framework.

### DevOps
* [Terraform](https://github.com/hashicorp/terraform) - For specifying IaC.
* [Docker](https://github.com/docker/cli) - For containerization.
* [Azure Devops Services](https://azure.microsoft.com/en-us/products/devops/) - For CICD.
* [Microsoft Azure](https://azure.microsoft.com/) - To host solution infrastructure.

## How to set up for development
### Prerequisites
- .net 6.0 must be installed.
- Docker must be installed.
- Node version 16.13.1 must be installed and the one used by npm.  
It is recommended to use Node version manager for this.
#### Setting up the HTTPS certificate
A HTTPS certificate with the password "password" __must__ be placed in this path:  
`%USERPROFILE%\.aspnet\https\aspnetapp.pfx`. This can be achieved easily by following these commands:  
1. `dotnet dev-certs https --clean` for removing existing certificate if there is one.
2. The next command depends on your OS:
    1. Windows: `dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p password -t`.  
    This command must be run from commandline and not something like git bash.
    2. Linux/macOS: `dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p password -t`

### Orchestrating for testing and development
#### Building the solution images
To orchestrate the containers needed for development, you first need to build all the solution images.  
This is done by running the `build-dockerfiles.sh` script in the root folder. This can take a couple  
of minutes the first time.
#### Container orchestration
After building the solution images, they can now be orchestrated via docker compose.  
There are four docker compose files in the root directory. These compose files all enable a different  
way to test/develop the solution. If you have followed the prior step, you are ready to use them.  
The following is a description of what each compose file is used for and when to use them:
1. `docker-compose.yml` - This file enables the orchestration of the full solution. This is ideal  
    if you do not have an IDE or just want to see the solution running.
2. `docker-compose-backend.yml` - This file enables the orchestration necessary for  
    backend development. It sets up MongoDB and frontend. Use this if you want to work on backend only.
3.  `docker-compose-backend-services-only` - `Same as docker-compose-backend.yml` but without frontend.  
    Used by the build pipeline.
4. `docker-compose-frontend-backend.yml` - This file enables the orchestration necessary for  
    frontend development. It sets up MongoDB and the backend API. Use this if you want to work  
    frontend only.
5. `docker-compose-fullstack.yml` - This file only orchestrates MongoDB. You should use this if  
    you intent to run both backend and frontend yourself. Ideal for fullstack development.

## Closing comments on the solution
This solution only presents one way to do things out of an infinite variety. During the development  
of this solution i've learned __a lot__ and have therefore become significantly more experienced as  
a developer. There are some choices that in this solution that i regret, like using JavaScript instead    
of TypeScript in the frontend app and using NoSQL instead of its relational counterpart. I also would  
have liked if the backend solution was cleaner and had a proper domain. However, i of course wouldn't  
have been able to make these conclusions if i hadn't tried things out and experimented 😊. 