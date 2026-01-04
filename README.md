# Speedrun Community (WPF)

A modern WPF application for managing speedrun records, built with clean MVVM architecture and Microsoft's Fluent Design System.

![Badge](https://img.shields.io/badge/Platform-WPF-blue) ![Badge](https://img.shields.io/badge/.NET-10.0-purple) ![Badge](https://img.shields.io/badge/UI-Fluent%20Design-0078D7)

## ✨ Features

- **Players** - Manage speedrun community members
- **Games** - Track games with platforms and categories
- **Results** - Record and browse speedrun times

## 🏗️ Technical Highlights

- **Architecture**: MVVM with Dependency Injection
- **Data Access**: Entity Framework Core (direct DbContext usage)
- **UI/UX**: [iNKORE UI.WPF.Modern](https://github.com/iNKORE-NET/UI.WPF.Modern) with Acrylic backdrop
- **Database**: SQLite (auto-created on first run)

## 💻 Tech Stack

| Component | Technology |
|-----------|------------|
| Framework | WPF (.NET 10) |
| Language | C# 14 |
| UI Library | iNKORE.UI.WPF.Modern |
| ORM | Entity Framework Core 10 |
| Database | SQLite or SQL Server |

## 🚀 Getting Started

1. **Clone** this repository
2. **Open** `SRC.sln` in Visual Studio 2022+
3. **Run** - The database is automatically created and seeded on first launch

> **Note**: No manual database setup required. `EnsureCreated()` handles everything.

---

*Reworked from a [legacy Windows Forms app](https://github.com/Mario64NP/SRC-PS) to demonstrate modern WPF standards.*
