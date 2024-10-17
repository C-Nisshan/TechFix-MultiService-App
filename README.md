# TechFix Multi-Service App  

## Overview  
TechFix Multi-Service App is a full-stack project that consists of:
1. **TechFix-backend**: Manages orders, products, and quotes with a SQL Server database.
2. **Supplier-backend**: Handles products, quotes, and client orders with synchronization between systems.
3. **TechFix-frontend**: React-based frontend for TechFix customers.
4. **Supplier-frontend**: React-based frontend for suppliers to manage products and quotes.

---

## Technologies Used  
- **Frontend**: React.js  
- **Backend**: ASP.NET Core 8.0  
- **Database**: Microsoft SQL Server  
- **Authentication**: JWT-based  
- **IDE**: Visual Studio 2022 for backend development  

---

## Setup Instructions

### Backend Setup
1. **Clone the repository:**
   ```bash
   git clone https://github.com/C-Nisshan/TechFix-MultiService-App.git
   cd TechFix-backend
   ```
2. **Open the solution in Visual Studio 2022:**
   - **Go to File → Open → Project/Solution and select the TechFix-Solution.sln.**
  
3. **Run database migrations:**
   ```bash
   Add-Migration InitialCreate
   Update-Database
   ```
4. **Start the backends:**
   - ***TechFix-backend: Runs on http://localhost:5000***
   - ***Supplier-backend: Runs on http://localhost:5001***

### Frontend Setup
1. **Navigate to the frontend directories and install dependencies:**
   ```bash
   cd techfix-frontend
   npm install

   cd ../supplier-frontend
   npm install
   ```
2. **Change React app ports (if needed): Modify the package.json scripts:**
   ```bash
   "start": "PORT=3001 react-scripts start"
   ```
3. **Run the React apps:**
   ```bash
   npm start
   ```
   - **TechFix-frontend: http://localhost:3000**
   - **Supplier-frontend: http://localhost:3001**






