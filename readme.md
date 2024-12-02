# 📚 API Documentation

## 🚀 Getting Started

### ✅ Prerequisites
- .NET SDK installed 🔧
- SQL Server installed and running 🗄️

### 💻 Installation
1. Clone the repository 🌿
2. Navigate to the project directory 🗺️
3. Run `dotnet restore` to download the required packages 📦
4. Update the connection string in `appsettings.json` to point to your SQL Server instance 🔗

### ▶️ Running the Project
1. Open the project in your preferred IDE (e.g., Visual Studio, VSCode) 🖥️
2. Run `dotnet run` to start the API 🏃‍♂️
3. The API will be accessible at `http://localhost:5000` 🌐

## 🌟 API Endpoints

### 🔑 Authentication
- `POST /auth/login` - User login. Returns a JWT token upon successful authentication. 🎟️
- `POST /auth/register` - User registration 📝

### 👥 Users
- `GET /users` - Get all users 👨‍👩‍👧‍👦
- `GET /users/{id}` - Get user by ID 🆔
- `POST /users` - Create a new user ✨
- `PUT /users/{id}` - Update user by ID 🔄
- `DELETE /users/{id}` - Delete user by ID ❌

## ⚠️ Error Handling
- All endpoints return appropriate HTTP status codes 🚦
- Errors are returned as JSON in the following format: 📜
  ```json
  {
    "error": "Error message"
  }
  ```

## 🔐 Authentication
- Authentication is performed using JWT tokens 🔒
- To authenticate, send a POST request to `/auth/login` with the user's email and password in the request body: 📧🔑
  ```json
  {
    "email": "user@example.com",
    "password": "password123"
  }
  ```
- If the credentials are valid, the API will respond with a JWT token: ✅
  ```json
  {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
  ```
- Include the token in the `Authorization` header as `Bearer <token>` for authenticated requests 🛡️
