# Agri-Energy Connect Platform Prototype

## Project Overview

This prototype is part of the Agri-Energy Connect initiative. It demonstrates a functional model of a web platform that connects farmers with green energy solutions and supports agricultural product management.

The system is built using ASP.NET Core MVC (.NET 8) with Entity Framework Core and Identity for authentication.

---

##  Development Environment Setup

### Prerequisites

* Visual Studio 2022
* .NET 8 SDK
* SQL Server LocalDB (installed with Visual Studio)

### Steps to Set Up:

1. Clone or download the repository.
2. Open the `.sln` file in Visual Studio 2022.
3. Restore NuGet packages if not already done.
4. Set `AgriEnergyConnect` as the startup project.
5. Press `F5` or click the **Start** button to run.

> The database will be created and seeded automatically on first run using `DbSeeder.cs`.

---

## How to Run the Prototype

1. Run the application using **IIS Express (HTTPS)**.
2. Navigate to `https://localhost:xxxx/` in your browser.
3. Use the **Register** link to create new Farmer accounts.
4. Use the seeded **admin account** to log in as an Employee:

   * Email: `admin@agrienergy.com`
   * Password: `Admin@123`

---

##  User Roles and Functionalities

### Farmer

* Can register and log in.
* Can add new products (name, category, production date).
* Can view a list of their own products.

### Employee

* Can log in using the admin credentials.
* Can view all products from all farmers.
* Can filter products by category, date, and farmer name.
* Can add new farmer profiles.

Role-based access control is enforced using ASP.NET Identity.

---

## Testing and Validation

* The application has been developed iteratively with regular testing.
* Role-based access and data flow have been verified per user.
* UX was designed to ensure ease of use and clarity for both technical and non-technical users.

---

## Project Structure

* `Models/` – Contains entity classes (`Farmer`, `Product`, `Employee`).
* `Controllers/` – Handles logic for managing products and farmers.
* `Views/` – Razor views, separated per controller.
* `Data/DbSeeder.cs` – Seeds initial data including roles, users, farmers, and products.
* `Areas/Identity/` – Manages user registration and login (via ASP.NET Identity).

---

## Notes

* Farmers are auto-linked to their products via email matching.
* Employees can assign a farmer when creating a product.
* Filters are implemented via query strings in the product index page.

---


