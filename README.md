# Tesko - Test Data Management System (TDMS)

A modern, .NET-based web application designed to streamline the request, approval, and tracking of telecom test assets (SIM cards, Vouchers, Devices) for QA teams.

## 🚀 Features

### 1. Role-Based Access Control (RBAC)
- **Requester (Tester):** View available stock, submit asset requests, track personal request history.
- **Approver (Test Lead/Manager):** View pending approvals, approve or reject requests with comments.
- **Admin:** Full control over inventory (CRUD), user management, and system configuration.

### 2. Core Modules
- **📦 Inventory Management:** 
    - Track SIMs, Vouchers, and Devices.
    - Real-time stock availability.
    - Visual low-stock indicators.
- **📝 Request & Approval Workflow:** 
    - Streamlined request submission.
    - Manager approval gate for controlled distribution.
- **👥 User Management:** 
    - Admin interface to Create, Read, Update, and Delete users.
    - Role assignment (Requester, Approver, Admin).
- **📊 Dashboard & Analytics:** 
    - Interactive charts for stock distribution.
    - "Top Requested Items" and "Top Requesters" insights.
    - Key metrics cards (Total Assets, Available, Low Stock).

### 3. Real-Time Updates
- **SignalR Integration:** Live dashboard updates without page refresh.
- **Event Broadcasting:** Changes to requests automatically notify all connected clients.
- **Persistent Data:** SQLite database ensures data persists across application restarts.

### 4. Modern UI/UX
- **Design System:** Professional "Enterprise Blue" theme using Bootstrap 5.
- **Responsive:** Fully responsive layout for desktop and mobile.
- **Interactive:** Chart.js integrations for data visualization with real-time updates.
- **Icons:** Comprehensive use of Bootstrap Icons for better visual navigation.

## 🛠️ Tech Stack

- **Framework:** .NET 10 (ASP.NET Core MVC)
- **Real-Time:** ASP.NET Core SignalR
- **Database:** SQLite (Lightweight, file-based, no external server required)
- **ORM:** Entity Framework Core 10.0
- **Authentication:** JWT (JSON Web Tokens) with Cookie-based storage
- **Frontend:** Razor Views, Bootstrap 5, Chart.js
- **Client Library:** SignalR JS Client

## 🏁 Getting Started

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)

### Installation & Run
1.  **Clone/Navigate to the project:**
    ```bash
    cd Tesko
    ```

2.  **Build the project:**
    ```bash
    dotnet build
    ```

3.  **Run the application:**
    ```bash
    dotnet run
    ```
    The application will start at `http://localhost:5283`.

**Note:** If using Scoop, use the full path:
```bash
& "C:\Users\gersum.asfaw\scoop\apps\dotnet10-sdk\current\dotnet.exe" run
```

Or set up a PowerShell alias for convenience (see [Setup Alias](#setup-powershell-alias) below).

### 🔐 Login Credentials (Seeded Data)
The application comes with pre-seeded users for testing:

| Role | Name | Email |
|------|------|-------|
| **Admin** | Admin User | admin@tesko.com |
| **Approver** | Test Lead | lead@tesko.com |
| **Requester** | Tester One | tester1@tesko.com |

*Note: The login page allows you to simulate login by selecting a user from a dropdown for development convenience.*

---

## 🔧 Setup PowerShell Alias

To avoid typing the long Scoop path every time, set up a PowerShell alias:

1.  Open PowerShell and run:
    ```powershell
    $profileDir = Split-Path $PROFILE
    if (-not (Test-Path $profileDir)) { New-Item -ItemType Directory -Path $profileDir -Force }
    New-Item -Path $PROFILE -ItemType File -Force
    notepad $PROFILE
    ```

2.  Add this line to the file:
    ```powershell
    Set-Alias dotnet "C:\Users\gersum.asfaw\scoop\apps\dotnet10-sdk\current\dotnet.exe"
    ```

3.  Save and close Notepad.

4.  Restart PowerShell and test:
    ```powershell
    dotnet --info
    ```

Now you can use `dotnet run` like a normal installation!

---

## 📊 Database Viewer

To view and manage your SQLite database in VS Code:

1.  **Install the Extension:**
   - Go to Extensions (`Ctrl+Shift+X`).
   - Search for and install **SQLite** by alexcvzz.

2.  **Open the Database:**
   - Press `Ctrl+Shift+P`.
   - Type `SQLite: Open Database` and select it.
   - Choose `Tesko.db` from the project root.

3.  **View Tables:**
   - Look for **SQLite Explorer** at the bottom of your Explorer pane.
   - Expand `Tesko.db` to see tables like `Requests`, `Users`, `Assets`, etc.

**Note:** Stop the running application before opening the database to avoid file locking issues.

---

## 📋 Features Walkthrough

### My Requests (Requester View)
- **Access:** `/Requests/Index`
- **Action:** Submit new asset requests, view request history with statuses (Pending, Approved, Rejected).

### Approval Queue (Approver/Admin View)
- **Access:** `/Requests/ApprovalQueue`
- **Action:** Review pending requests, approve or reject with optional comments.
- **Impact:** Approved requests update inventory stock in real-time.

### Approval History (All Users)
- **Access:** `/Requests/History`
- **Admin/Approver:** Can view **all** approval and rejection records system-wide.
- **Requester:** Can view only their own approval/rejection history.

### Inventory Management (Admin Only)
- **Access:** `/Assets/Index`
- **Action:** Create, Read, Update, Delete assets. Manage stock levels and low-stock thresholds.

### User Management (Admin Only)
- **Access:** `/Users/Index`
- **Action:** Create, Read, Update, Delete users. Assign roles (Requester, Approver, Admin).

### Dashboard (All Authenticated Users)
- **Access:** `/` (Home)
- **Real-Time Updates:** Charts and metrics update automatically when requests are created/approved.
- **Metrics:** Total Assets, Available Stock, Pending Requests, Low Stock Items.
- **Charts:** Top Requested Items, Top Requesters.

---

## 🌐 Real-Time Features

### SignalR Hub
- **Endpoint:** `/dashboardHub`
- **Message:** `ReceiveDashboardUpdate` broadcast after request lifecycle events (Create, Approve, Reject).
- **Client-Side:** Automatic fetch from `/Home/GetDashboardData` and DOM updates without page refresh.

### Testing Real-Time Updates
1.  Open the dashboard in one browser tab.
2.  In another tab, create/approve a request.
3.  Watch the first tab update automatically (no page refresh needed).

---

## 🔐 Authentication & Authorization

- **JWT Tokens:** Stored securely in HTTP-only cookies.
- **Token Expiry:** 1 hour (configurable in `appsettings.json`).
- **Role-Based Routes:** Admin and Approver routes are protected via `[Authorize]` attributes.
- **Redirect Logic:** Unauthorized access redirects to `/Auth/Login`.

---

## 📂 Project Structure

```
Tesko/
├── Controllers/          # MVC Controllers
│   ├── AuthController.cs
│   ├── HomeController.cs
│   ├── RequestsController.cs
│   ├── AssetsController.cs
│   └── UsersController.cs
├── Models/               # Data Models
│   ├── User.cs
│   ├── Asset.cs
│   ├── Request.cs
│   ├── AuditLog.cs
│   └── ViewModels/
├── Views/                # Razor Views
│   ├── Home/
│   ├── Requests/
│   ├── Assets/
│   ├── Users/
│   ├── Auth/
│   └── Shared/
├── Data/                 # EF Core DbContext
│   └── TeskoDbContext.cs
├── Hubs/                 # SignalR Hubs
│   └── DashboardHub.cs
├── wwwroot/              # Static files (CSS, JS)
├── Program.cs            # Application entry point
├── appsettings.json      # Configuration
└── Tesko.csproj          # Project file
```

---

## 📝 Database Schema

### Users
- `Id` (int, PK)
- `Name` (string)
- `Email` (string)
- `Role` (string: "Requester", "Approver", "Admin")

### Assets
- `Id` (int, PK)
- `Name` (string)
- `Type` (string)
- `TotalStock` (int)
- `AvailableStock` (int)
- `AllocatedStock` (int)
- `DefectiveStock` (int)
- `LowStockThreshold` (int)

### Requests
- `Id` (int, PK)
- `UserId` (int, FK → Users)
- `AssetId` (int, FK → Assets)
- `Quantity` (int)
- `Purpose` (string)
- `Status` (string: "Pending", "Approved", "Rejected")
- `RequestDate` (DateTime)
- `ActionDate` (DateTime, nullable)
- `ApproverId` (int, nullable, FK → Users)
- `Comment` (string, nullable)

### AuditLogs
- `Id` (int, PK)
- `UserId` (int, FK → Users)
- `Action` (string)
- `Details` (string)
- `Timestamp` (DateTime)

---

## 📜 Changelog & Versioning

### v1.4 - Real-Time Updates & Data Persistence
- **SignalR Integration:** Live dashboard updates without page refresh.
- **Dashboard API:** New `/Home/GetDashboardData` endpoint for client-side updates.
- **Data Persistence:** Fixed SQLite database path to ensure data persists across restarts.
- **Approval History:** New `/Requests/History` page with role-based filtering.
- **Bug Fixes:** Resolved file locking issues with database initialization.

### v1.3 - UI/UX Overhaul & Security
- **Authentication:** Implemented JWT-based authentication.
- **Security:** Added `[Authorize]` attributes to controllers; secured Admin routes.
- **UI:** Complete redesign with a modern, professional look.
- **Navigation:** Dynamic navbar based on user claims/roles.

### v1.2 - User Management
- **Feature:** Added full CRUD capabilities for Users.
- **Admin:** Dedicated "Users" controller and views.

### v1.1 - Database Migration
- **Infrastructure:** Switched from SQL Server to SQLite for better portability and ease of setup.

### v1.0 - Initial Release
- Basic Inventory Management.
- Request creation and approval workflow.
- Basic Dashboard.

---

## 🤝 Contributing

To extend this project:

1. Create a new branch for your feature.
2. Make your changes (add new models, controllers, views as needed).
3. Test thoroughly in both the dashboard and specific features.
4. Submit a pull request with a clear description.

---

## 📞 Support & Troubleshooting

### Common Issues

**Q: Database keeps resetting on restart?**
- **A:** Ensure the connection string points to the project root. Check `Program.cs` line 15 to confirm `Tesko.db` is created in the correct location.

**Q: "No SDKs found" error?**
- **A:** You're likely using Scoop. See [Setup PowerShell Alias](#setup-powershell-alias) to simplify running `dotnet` commands.

**Q: SQLite extension showing file locked error?**
- **A:** Stop the running application first (`Ctrl+C` in the terminal) before viewing the database in VS Code.

**Q: Real-time updates not working?**
- **A:** Ensure SignalR is connected by checking browser console for errors. Make sure `/dashboardHub` route is mapped in `Program.cs`.

---

*Maintained by the Tesko Development Team. Last Updated: November 26, 2025*
