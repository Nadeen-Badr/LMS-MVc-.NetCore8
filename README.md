# Library Management System

A feature-rich Library Management System built using .NET MVC. This application allows users to browse, search, and filter books, as well as manage their rental and sales status. It is designed to demonstrate backend development skills using .NET and includes advanced features like translations, charts, and a clean user interface.

## Features

### Core Features
- **Book Management**: Display books with images, categories, and status (Available, Rented, Sold).
- **Filtering & Searching**: Filter books by category, status, and author. Search books by name or keywords.
- **Rent and Sell**: Rent books for a user-defined duration or mark books as sold.
- **Translations**: Multi-language support for the user interface.
- **Charts**: Visualize book statuses and revenue analysis using charts.

### Admin-Specific Features
- View charts for:
  - Books categorized as Available, Rented, and Sold.
  - Revenue from rentals and sales.

### UI/UX Enhancements
- Modern and responsive design using Bootstrap.
- Styled pages for enhanced user experience.

## Project Structure
The project follows the **Repository Pattern** for better code organization and separation of concerns:

- **Controllers**: Handle HTTP requests and pass data to views.
- **Models**: Represent application data and business logic.
- **Repositories**:
  - `BookRepository`: Handles book-related database operations.
  - `CategoryRepository`: Manages categories.
- **Views**: Razor views for rendering the user interface.
- **Interfaces**: Define contracts for repositories.

## Technologies Used

- **Framework**: .NET MVC
- **Languages**: C#, HTML, CSS, JavaScript
- **Frontend**: Bootstrap for styling
- **Database**: Entity Framework for data access
- **Tools**: Visual Studio Code, Git

## How to Run the Project

### Prerequisites
1. Install [.NET SDK](https://dotnet.microsoft.com/download) (version used: `x.x.x`).
2. Install a database server (e.g., SQL Server or use SQLite for development).

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/library-management-system.git
   cd library-management-system
   ```
2. Restore NuGet packages:
   ```bash
   dotnet restore
   ```
3. Update the database connection string in `appsettings.json`.
4. Apply migrations and seed the database:
   ```bash
   dotnet ef database update
   ```
5. Run the application:
   ```bash
   dotnet run
   ```
6. Open your browser and navigate to `https://localhost:5001`.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.

## Acknowledgments
- Bootstrap for CSS framework.
- .NET documentation and tutorials for guidance.

---
Feel free to contribute by submitting issues or pull requests!

