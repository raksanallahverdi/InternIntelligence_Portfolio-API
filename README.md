Portfolio API Development
This project is a RESTful API designed for managing portfolio content, including projects, skills, achievements, and contact form submissions. It allows users to submit their portfolio data and manage it through CRUD (Create, Read, Update, Delete) operations. Admins can also view all submitted contact forms.

Key Features
🔒 Secure User Authentication: Uses authentication mechanisms to ensure secure access to the management endpoints.
🔄 Full CRUD Support: Allows users to create, read, update, and delete projects, skills, achievements, and contact forms.
⚙️ ASP.NET Core: Built with ASP.NET Core for high performance and scalability.
🗄️ SQL Server Integration: Uses SQL Server for storing portfolio data with a structured relational approach.
🌐 RESTful API Design: The API follows RESTful principles, ensuring compatibility with modern web standards.
🛡️ Data Validation & Error Handling: Includes input validation and proper error handling to ensure the integrity of the data and user-friendly feedback.
💬 Contact Form Submission: Allows users to submit contact forms, which can be retrieved by admins.
Technologies Used
Backend: ASP.NET Core 6
Database: SQL Server
ORM: Entity Framework Core
API Design: RESTful Principles
Automapper: Used to map DTOs to entities and vice versa
Swagger: API documentation for easy exploration
Authentication: Role-based access for managing portfolio content
Security: Input validation and error handling
Endpoints
Project Endpoints
POST /projects: Create a new project.
GET /projects: Get all projects.
GET /projects/{id}: Get a specific project by ID.
PUT /projects/{id}: Update a project by ID.
DELETE /projects/{id}: Delete a project by ID.
Skill Endpoints
POST /skills: Create a new skill entry.
GET /skills: Get all skills.
GET /skills/{id}: Get a specific skill by ID.
PUT /skills/{id}: Update a skill by ID.
DELETE /skills/{id}: Delete a skill by ID.
Achievement Endpoints
POST /achievements: Create a new achievement.
GET /achievements: Get all achievements.
GET /achievements/{id}: Get a specific achievement by ID.
PUT /achievements/{id}: Update an achievement by ID.
DELETE /achievements/{id}: Delete an achievement by ID.
Contact Form Endpoints
POST /contact: Submit a contact form.
GET /contact: Get all submitted contact forms (admin access).
How to Run the Project
Prerequisites:
.NET 6 SDK installed
SQL Server or local database setup
Steps:
Clone the repository:

bash
Copy
Edit
git clone https://github.com/your-repository/portfolio-api.git
cd portfolio-api
Open the project in Visual Studio or use the command line to restore dependencies:

bash
Copy
Edit
dotnet restore
Update your appsettings.json file to configure your connection string for SQL Server.

Run the migrations to create the database schema:

bash
Copy
Edit
dotnet ef database update
Run the application:

bash
Copy
Edit
dotnet run
The API will be hosted locally, typically at http://localhost:5000.

Admin Access:
Admin users will have special permissions to view and manage submitted contact forms.
API Documentation
You can explore the API using Swagger at http://localhost:5000/swagger once the application is running.

Contribution
Feel free to fork this repository and submit pull requests for any features or improvements.
