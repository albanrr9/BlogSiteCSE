# BlogCSE 🖥️📝
note: Project in progress!

[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-green)](https://opensource.org/licenses/MIT)
[![GitHub Issues](https://img.shields.io/github/issues/edonberishaa/BlogCSE)](https://github.com/edonberishaa/BlogCSE/issues)

A collaborative ASP.NET Core blog platform for computer science and engineering enthusiasts.

![Blog Preview](https://via.placeholder.com/800x400.png?text=BlogCSE+Preview) <!-- Add real screenshot later -->

## Features ✨
- 📝 Create and manage technical blog posts
- 🔖 Tag system for C#/.NET, Algorithms, Web Development, etc.
- 🔍 Full-text search functionality
- 👥 User authentication & authorization
- 💬 Comment system for discussions
- 📈 Analytics dashboard (WIP)
- 🌐 Responsive UI with Bootstrap

## Tech Stack 🛠️
- **Backend**: ASP.NET Core MVC
- **Database**: Entity Framework Core + SQL Server (Azure-hosted)
- **Frontend**: Razor Pages, Bootstrap 5, JavaScript
- **Authentication**: ASP.NET Core Identity
- **Real-Time**: SignalR (for future updates)
- **Tools**: Git, Azure DevOps, Visual Studio 2022

## Getting Started 🚀

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- SQL Server Management Studio (SSMS)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/edonberishaa/BlogCSE.git
   cd BlogCSE
   
2. Configure the database:
   - Update connection string in appsettings.json:
     ```json
     "ConnectionStrings": {
     "DefaultConnection": "Server=your-server;Database=BlogCSE;Trusted_Connection=True;"
     }
3. Run database migrations:
   ```bash
   dotnet ef database update
4. Start the application:
   ```bash
   dotnet run
5. Visit `https://localhost:7274` in your browser
## Contributing  🤝
We welcome contributions! Please follow these steps:
1. Fork the repository
2. Create your feature branch:
   ```bash
   git checkout -b feature/amazing-feature
3. Commit your changes:
   ```bash
   git commit -m 'Add some amazing feature'
4. Push to the branch:
   ```bash
   git push origin feature/amazing-feature
5. Open a Pull Request
## License 📄
This project is licensed under the MIT License.
## Acknowledgments 🎓
- [Edon Berisha](https://github.com/edonberishaa/) - Project Maintainer
- [Alban Rrahmani](https://github.com/albanrr9) - Core Contributor

**Happy Coding! 👨💻👩💻**
