# raceday.api-programming-part-2
raceday
# RaceDay Event Management System – Part 2

## 1. Project Overview

The RaceDay Event Management System is an ASP.NET Core Web API developed to manage race events, event categories, participant enrolments, participant profiles, and race results.

The system provides secure access to features based on user roles. Organisers can manage race events and categories, while participants can view events, enrol in categories, manage their profiles, and view their results.

## 2. Technologies Used

- C#
- ASP.NET Core Web API
- .NET 8
- Entity Framework Core
- MySQL
- JWT (JSON Web Token) authentication
- Swagger / OpenAPI
- Visual Studio
- Git and GitHub

## 3. Main Features

### User Authentication
- Register a new account.
- Log in using registered credentials.
- Store passwords as secure hashes.
- Authenticate API requests using JWT tokens.
- Restrict access according to user roles.

### Event Management
- Create, view, update, and delete race events.
- Associate events with organisers.
- View available race events.

### Category Management
- Create and manage race categories.
- View categories associated with events.

### Participant Enrolments
- Enrol participants in race categories.
- View enrolment records.
- Apply access restrictions to enrolment operations.

### Participant Profiles
- Create and manage participant profile information.
- Retrieve participant profile records.

### Race Results
- Record and update race results.
- Store finishing times and positions.
- Allow participants to access their results according to the implemented authorization rules.

## 4. User Roles

**Organiser**
- Manage race events.
- Manage event categories.
- View relevant participant enrolments.
- Record and update race results.

**Participant**
- Register and log in.
- View available race events and categories.
- Enrol in race categories.
- Manage participant profile information.
- View their race results.

## 5. Requirements

Install the following software before running the project:

- Visual Studio 2022 or a compatible IDE.
- .NET 8 SDK.
- MySQL Server.
- MySQL Workbench (optional).
- Git.

## 6. How to Run the Project

1. Clone or download this repository.
2. Open the RaceDay solution (`.sln`) in Visual Studio.
3. Restore the NuGet packages.
4. Configure the MySQL connection string using your local development configuration.
5. Configure the JWT authentication settings.
6. Ensure the database and required tables are available.
7. Build the solution.
8. Run the API using Visual Studio.
9. Open the Swagger URL shown in the browser or application output.

## 7. API Testing

Use Swagger to test the available API endpoints.

Test the following operations where implemented:

- User registration and login.
- Retrieving authenticated user information.
- Creating and viewing events.
- Creating and viewing categories.
- Creating and viewing enrolments.
- Managing participant profiles.
- Recording and viewing race results.
- Authorization for Organiser and Participant roles.
- Validation errors and duplicate registration attempts.

For protected endpoints, log in and provide the JWT token using Swagger's **Authorize** feature.

## 8. Security

- Passwords should be stored as hashes rather than plain text.
- Protected endpoints should require valid authentication.
- Role-based authorization should restrict access to appropriate users.
- Database passwords and JWT signing secrets must not be committed to GitHub.
- Production deployments should use secure configuration and HTTPS.

## 9. Testing and Continuous Integration

The project should include unit tests for important functionality, such as:

- User registration and login.
- Event management.
- Role-based access restrictions.
- Participant enrolments.
- Race result management.

GitHub Actions can be configured to build the solution and run the automated tests whenever changes are pushed to the repository.

**Build status:** Add your verified GitHub Actions build badge or screenshot after the workflow has run successfully.

## 10. Repository

GitHub repository: Add your repository URL here.

## 11. Demonstration Video

YouTube demonstration: Add your unlisted YouTube video link here after recording the project demonstration.

## 12. Author

Name: Add your full name here.

Course: PROG6212 – Programming 2B

Project: RaceDay Event Management System – Part 2
