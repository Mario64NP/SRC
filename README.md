# Speedrunning Community (WPF)

A WPF rework of the original Windows Forms CRUD application for managing speedrun records across different games and categories. This version introduces a more modern UI and a cleaner backend structure using Entity Framework and code-first migrations.

---

## 🕹️ Features

- Keep track of, add, edit, and delete speedrun records, players, games, categories, and platforms
- Uses Entity Framework for ORM with code-first migrations
- Implements Repository and Unit of Work design patterns
- Structured relational database model
- Improved separation of concerns and maintainability

---

## 💻 Tech Stack

- **Framework:** WPF (.NET)
- **Language:** C#
- **ORM:** Entity Framework
- **Database:** SQL Server (local instance)

---

## 🗂️ Data Structure

- **Players**: Nick, Age  
- **Games**: Title, Genre, Platform  
- **Platforms**: Name  
- **Categories**: Name, Description  
- **Records**: Player, Game, Category, Timer, Date

---

## 🚀 How to Run

1. Clone this repository:

    ```bash
    git clone https://github.com/Mario64NP/SRC.git
    ```

2. Open the `.sln` file in Visual Studio

3. Ensure your local SQL Server instance is running

4. Run Entity Framework migrations to initialize the database:

    ```bash
    Update-Database
    ```

5. Build and run the app via Visual Studio

---

## 📦 Notes

This version is a rework of the original [Windows Forms version](https://github.com/Mario64NP/SRC-PS), with improvements in architecture, maintainability, and tooling.
