# 🚀 NextERP Setup Guide

This guide will help you set up and run the **NextERP** project on your local machine.

---

## 📦 Requirements

Before starting, make sure you have installed:

- 🟪 .NET 9 SDK  
- 💻 Visual Studio 2022 (ensure it supports .NET 9)  
- 🗄️ Microsoft SQL Server 2022  

---

## 🛢️ Database Scripts

This folder contains SQL scripts used for creating and initializing the **NextERP** database.

### 📂 Files
- 📜 **script.sql** → Combined script for quick setup.

### ⚡ How to Use
1. 🖥️ Open SQL Server Management Studio (SSMS) or Azure Data Studio.  
2. ▶️ Run the `script.sql` file to create and initialize the database.  
3. 🔧 Update the connection string in `appsettings.json` of your project:  

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=NextERP;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
   }
