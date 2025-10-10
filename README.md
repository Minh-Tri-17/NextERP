# 🚀 NextERP Setup Guide

This guide will help you set up and run the **NextERP** project on your local machine.

---

## 📦 Requirements

Before starting, make sure you have installed:

* 🟪 .NET 9 SDK
* 💻 Visual Studio 2022 (ensure it supports .NET 9)
* 🗄️ Microsoft SQL Server 2022

---

## 🛢️ Database Scripts

This folder contains SQL scripts used for creating and initializing the **NextERP** database.

### 📂 Files

* 📜 **DAL → SQL → script.sql** → Combined script for quick setup.

### ⚡ How to Use

1. 🖥️ Open SQL Server Management Studio (SSMS) or Azure Data Studio.
2. ▶️ Run the `script.sql` file to create and initialize the database.
3. 🔧 Update the connection string in `appsettings.json` of your project:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=NextERP;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
   }
   ```

---

## 🔑 API Settings

In your **API project** `appsettings.json`, configure authentication and mail settings:

```json
"Tokens": {
  "Key": "a8D#4x!2@Lpq9$Km*E3Z5^N7&wHT0vC+oYjRUI6MsdgBX1!fzQ",
  "Issuer": "your contact"
},
"ConfigMail": {
  "SMTPHost": "smtp.gmail.com",
  "SMTPPort": 587,
  "FromEmail": "youremail@gmail.com",
  "EmailPassword": "your-app-password",
  "FromName": "your name"
}
```

* **Key** → Secret key for JWT authentication (keep it secure).
* **Issuer** → Can be your name, company, or contact info.
* **SMTP settings** → Required if you want the system to send emails (e.g., password reset, notifications). Use an app-specific password for Gmail.

---

## 🌐 MVC Settings

In your **MVC project** `appsettings.json`, configure API access and authentication:

```json
"APIAddress": "https://localhost:20258",
"Tokens": {
  "Key": "a8D#4x!2@Lpq9$Km*E3Z5^N7&wHT0vC+oYjRUI6MsdgBX1!fzQ",
  "Issuer": "your contact"
}
```

* **APIAddress** → The URL of your API (adjust the port to match your running API).
* **Tokens** → Must match the `Key` and `Issuer` defined in your API project for JWT authentication to work properly.

---

✅ After completing these steps, your **NextERP** should be ready to run locally!
