# Training Management System

## Overview

A comprehensive ASP.NET Core MVC web application designed for managing training programs, including courses, sessions, users, and grades. This system implements modern software engineering practices including the Repository Pattern, comprehensive validation, ASP.NET Core Identity authentication, and role-based authorization with hierarchical permissions.

## 🚀 Features

### 🔐 Authentication & Authorization System

#### **Full Authentication Implementation**
- **ASP.NET Core Identity Integration** - Secure user authentication and session management
- **Hierarchical Role System** - Four-tier permission structure with inheritance
- **Password Security** - Enforced complexity requirements and lockout protection
- **Session Management** - Configurable timeout and remember-me functionality

#### **Role Hierarchy & Permissions**
- **Super Admin** (Level 3) - Full system control including admin management
- **Admin** (Level 2) - System administration excluding super admin functions
- **Instructor** (Level 1) - Course and session management capabilities
- **Trainee** (Level 0) - Basic access for learning and grade viewing

#### **Security Features**
- **Account Lockout** - Automatic lockout after failed login attempts
- **Password Requirements** - Minimum complexity and length enforcement
- **Secure Sessions** - HTTP-only cookies with sliding expiration
- **Access Control** - Page-level and action-level authorization
- **Audit Trail** - Login/logout tracking and user activity monitoring

#### **User Management**
- **Role-Based Registration** - Admins create accounts with appropriate permissions
- **Profile Management** - Users can view and update their profiles
- **Password Management** - Secure password change functionality
- **Account Status Control** - Activate/deactivate user accounts

### Core Functionality

#### 🎓 Course Management
- **Create, Read, Update, Delete** courses
- **Instructor Assignment** - Assign qualified instructors to courses
- **Advanced Search** - Search courses by name or category
- **Unique Validation** - Ensures course names are unique across the system

**Validation Rules:**
- Name: Required, 3–50 characters, must be unique
- Category: Required field for organization
- Instructor: Optional assignment to qualified users

#### 📅 Session Management
- **Schedule Management** - Create and manage training sessions for courses
- **Date Validation** - Prevents scheduling in the past
- **Duration Control** - End date must be after start date
- **Course Association** - Link sessions to specific courses
- **Search Functionality** - Find sessions by course name

**Validation Rules:**
- StartDate: Required, cannot be in the past
- EndDate: Required, must be after StartDate
- CourseId: Required association with existing course

#### 👥 User Management
- **Multi-Role System** - Support for Admin, Instructor, and Trainee roles
- **User CRUD Operations** - Complete user lifecycle management
- **Email Validation** - Ensures unique, valid email addresses
- **Role-Based Features** - Different capabilities based on user role

**User Roles:**
- **Admin**: Full system access and management capabilities
- **Instructor**: Course and session management for assigned courses
- **Trainee**: Session participation and grade viewing

**Validation Rules:**
- Name: Required, 3–50 characters
- Email: Required, valid email format, must be unique
- Role: Required selection from defined roles

#### 📊 Grades Management
- **Performance Tracking** - Record grades for trainees in sessions
- **Grade Reports** - View individual trainee performance
- **Validation Controls** - Ensures grades are within acceptable range
- **Session Association** - Links grades to specific training sessions

**Validation Rules:**
- Value: Required, must be between 0 and 100
- SessionId: Required association with existing session
- TraineeId: Required association with trainee user

## 🏗️ Technical Architecture

### Design Patterns
- **Repository Pattern**: Abstracts data access logic for testability and maintainability
- **Dependency Injection**: Promotes loose coupling and easier testing
- **MVC Pattern**: Separates concerns between Model, View, and Controller layers

### Authentication & Security Architecture
- **ASP.NET Core Identity**: Complete authentication and authorization framework
- **Role-Based Authorization**: Hierarchical permission system with four user levels
- **Custom Authorization Attributes**: Fine-grained access control decorators
- **Security Headers**: HTTPS enforcement, secure cookies, and HSTS
- **Password Security**: Complexity requirements, hashing, and lockout protection

### Authorization Attributes
- `[MinimumRole(UserRole)]`: Hierarchical role checking with inheritance
- `[RequireRole(UserRole[])]`: Exact role match requirement
- `[SuperAdminOnly]`: Highest privilege level access
- `[AdminOrAbove]`: Admin and SuperAdmin access
- `[InstructorOrAbove]`: Instructor, Admin, and SuperAdmin access
- `[ResourceOwnerOrAdmin]`: Own resource or admin access

### Data Models

```csharp
// Authentication entities
ApplicationUser: Id, UserName, Email, FullName, Role, IsActive, CreatedAt
IdentityRole: Id, Name, NormalizedName

// Core entities with their relationships
Course: Id, Name, Category, InstructorId
Session: Id, CourseId, StartDate, EndDate
User: Id, Name, Email, Role (Legacy model for backward compatibility)
Grade: Id, SessionId, TraineeId, Value
```

### Validation Strategy
- **Data Annotations**: Standard validation attributes for common rules
- **Custom Validation**: Business-specific validation attributes
- **Client-Side Validation**: Real-time feedback for better user experience
- **Server-Side Validation**: Comprehensive validation as security backstop

### Custom Validation Attributes
- `FutureDateAttribute`: Ensures dates are not in the past
- `DateGreaterThanAttribute`: Compares two date properties
- `UniqueCourseNameAttribute`: Prevents duplicate course names
- `UniqueEmailAttribute`: Ensures email uniqueness

## 🧪 Testing the Authentication System

### Quick Start Test

1. **Access the Application**
   - Open `http://localhost:5069` in your browser
   - You should see the home page with "Login" in the navigation

2. **Test SuperAdmin Login**
   - Click "Login"
   - Enter credentials:
     - Email: `superadmin@trainingms.com`
     - Password: `SuperAdmin123!`
   - After login, you should see "User Management" in the navigation

3. **Test User Management**
   - Click "User Management" to see all users
   - Click "Register New User" to create additional accounts
   - Test creating Admin, Instructor, and Trainee accounts

4. **Test Role-Based Access**
   - Log out and log in as different roles
   - Verify that menu items change based on user permissions
   - Test accessing restricted pages directly via URL

### Authentication Features Verification

- ✅ Secure password hashing with ASP.NET Core Identity
- ✅ Role-based navigation menus
- ✅ Hierarchical permission system
- ✅ User account activation/deactivation
- ✅ Profile management and password changes
- ✅ Automatic redirect to login for protected pages
- ✅ Custom authorization attributes working correctly

## 🛠️ Technology Stack

- **Framework**: ASP.NET Core 8.0 MVC
- **Database**: SQL Server with Entity Framework Core
- **Frontend**: Razor Views with Bootstrap styling
- **Validation**: Data Annotations + Custom Attributes
- **Architecture**: Repository Pattern with Dependency Injection

## 📋 Prerequisites

- .NET 8.0 SDK or later
- SQL Server (LocalDB, Express, or Full version)
- Visual Studio 2022 or VS Code
- Git for version control

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/Ahmedfahmy8308/Training-Management-System-ITI-Project.git
cd Training-Management-System-ITI-Project
```

### 2. Configure Database Connection
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TrainingManagementDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 3. Restore Dependencies
```bash
dotnet restore
```

### 4. Initialize Database and Authentication

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

The application will start and automatically create the database with a default SuperAdmin user on first run.

**Default SuperAdmin Credentials:**
- **Email**: superadmin@trainingms.com
- **Password**: SuperAdmin123!
- **Role**: SuperAdmin (Full system access)

### 6. Access the Application

Open your browser and navigate to `https://localhost:5001` or `http://localhost:5000`

**First Login Steps:**
1. Click "Login" in the navigation menu
2. Use the SuperAdmin credentials above
3. You can now create additional Admin and Instructor accounts
4. Regular Trainees can be registered through the user management interface

## 📁 Project Structure

```
Training-Management-System-ITI-Project/
├── Controllers/           # MVC Controllers handling HTTP requests
├── Models/               # Entity models and business logic
├── Views/                # Razor view templates
├── ViewModels/           # Data transfer objects for views
├── Repositories/         # Data access layer implementation
├── Attributes/           # Custom validation attributes
├── Data/                 # Entity Framework DbContext
├── Migrations/           # Database schema migrations
├── wwwroot/             # Static files (CSS, JS, images)
└── Program.cs           # Application entry point and configuration
```

## 🎯 Usage Examples

## 🎯 Usage Guide

### Authentication & User Management

#### First-Time Login

1. Navigate to the application URL (`https://localhost:5001`)
2. Click **"Login"** in the top navigation
3. Use the default SuperAdmin credentials:
   - Email: `superadmin@trainingms.com`
   - Password: `SuperAdmin123!`

#### Creating New Users

**SuperAdmin and Admin users can create accounts for:**
- Other Admins
- Instructors
- Trainees

**Steps to create a new user:**

1. After logging in, click **"User Management"** in the navigation
2. Click **"Register New User"**
3. Fill in the user details:
   - Full Name
   - Email Address
   - Select Role (Admin/Instructor/Trainee)
   - Password (must meet complexity requirements)
4. Click **"Register"** to create the account

#### Role Hierarchy & Permissions

**SuperAdmin (Highest Access)**
- Full system access
- Manage all users, courses, sessions, and grades
- System administration capabilities

**Admin**
- Manage courses, sessions, and grades
- Create Instructor and Trainee accounts
- View all system data

**Instructor**
- Create and manage their own courses
- Schedule sessions for their courses
- Record grades for their sessions
- View trainee progress

**Trainee (Limited Access)**
- View enrolled courses and sessions
- View own grades and progress
- Update profile information

#### User Account Management

**Change Password:**

1. Click your name in the top-right corner
2. Select **"Profile"**
3. Click **"Change Password"**
4. Enter current password and new password
5. Click **"Update Password"**

**Manage Users (Admin/SuperAdmin only):**

1. Navigate to **"User Management"**
2. View all users with their roles and status
3. **Activate/Deactivate** users using the toggle button
4. Search users by name or email
5. Filter users by role

### Creating a Course
1. Navigate to **Courses** → **Create New**
2. Enter course name (unique, 3-50 characters)
3. Select or enter course category
4. Optionally assign an instructor
5. Save the course

### Scheduling a Session
1. Navigate to **Sessions** → **Create New**
2. Select an existing course
3. Set start date (cannot be in the past)
4. Set end date (must be after start date)
5. Save the session

### Recording Grades
1. Navigate to **Grades** → **Create New**
2. Select a session
3. Choose a trainee
4. Enter grade value (0-100)
5. Save the grade

## 🔧 Configuration

### Database Settings
The application uses Entity Framework Core with SQL Server. The database is automatically created on first run with the schema defined by the models.

### Environment Configuration
- **Development**: Detailed error pages and developer tools
- **Production**: Secure error handling and performance optimizations

## 🧪 Testing

The application includes comprehensive validation testing through:
- Data annotation validation
- Custom validation attributes
- Controller action validation
- Client-side validation feedback

## 📈 Future Enhancements

- **Authentication & Authorization**: Implement user login/logout functionality
- **Reporting Dashboard**: Advanced analytics and reporting features
- **Email Notifications**: Automated session reminders and grade notifications
- **File Upload**: Support for course materials and assignments
- **API Development**: RESTful API for mobile applications
- **Advanced Search**: Filter by date ranges, instructor, and more criteria

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 📧 Contact

**Project Maintainer**: Ahmed Fahmy  
**Email**: [Contact through GitHub](https://github.com/Ahmedfahmy8308)  
**Project Link**: [https://github.com/Ahmedfahmy8308/Training-Management-System-ITI-Project](https://github.com/Ahmedfahmy8308/Training-Management-System-ITI-Project)

## 🙏 Acknowledgments

- ITI (Information Technology Institute) for project requirements and guidance
- ASP.NET Core team for the excellent framework
- Entity Framework team for the robust ORM solution
- Bootstrap team for the responsive UI framework

---

*Built with ❤️ using ASP.NET Core MVC*