📅 Booking System

A simple web-based booking system built with ASP.NET Core MVC and Entity Framework Core. This system allows users to manage resources and create, edit, or delete bookings for those resources.
✅ Features

    User authentication and authorization

    CRUD operations for resources and bookings

    Booking conflict detection

    Dashboard with filtering options

    Prevent deletion of resources with active bookings

🛠️ Requirements

    .NET 9 SDK

    SQL Server (local or remote)

    Visual Studio 2022+ or VS Code (optional)

🚀 Getting Started
1. Clone the Repository

git clone https://github.com/nonkosii/Booking_System.git
cd booking-system

2. Update Connection String

Open appsettings.json and configure the database connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=(local);Database=Booking_System;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}

    Replace it with your own SQL Server details if needed.

3. Run Database Migrations

Ensure Entity Framework tools are installed:

dotnet tool install --global dotnet-ef

Run the migrations to create the database schema:

dotnet ef database update

4. Alternative restore the backup database in this src of this application

4. Run the Application

Use the CLI:

dotnet run

Or from Visual Studio:

    Open the solution .sln file

    Press F5 to build and run

🔐 User Authentication

Authentication is enabled. You’ll need to register a new account or log in using Identity.

    You can modify or seed admin users if needed.

📂 Project Structure

/Controllers       → MVC Controllers
/Models            → Data Models
/Services          → Business Logic (e.g., Booking/Resource conflict rules)
/Data              → DbContext & Migrations
/Views             → Razor Views

📌 Notes

    The app prevents overlapping bookings.

    You can’t delete a resource that has future bookings.

    The dashboard allows filtering bookings by date, resource, or keywords.

🧪 Testing

Basic validation is handled server-side via ModelState. You can add unit tests for services and controller logic if needed.
🤝 Contributions

Feel free to fork and submit pull requests!
📃 License

MIT License – free to use and modify.