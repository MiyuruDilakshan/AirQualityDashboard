# 🌍 Real-time Air Quality Monitoring Dashboard for Colombo

### 📌 Overview
This web-based **Real-time Air Quality Monitoring Dashboard** was developed as part of the **PUSL2020 coursework**. It simulates real-time air quality data for **Colombo** using **synthetic data generation**, providing users with **dynamic visualizations**, **real-time alerts**, and **historical analytics**.

### 🚀 Features
- 📊 **Interactive Map (Leaflet)** – Visualize air quality data in real time.
- ⚡ **Real-time Alerts** – Get notified when air quality levels exceed set thresholds.
- 📈 **Historical Data Analytics** – Track trends and patterns in air quality.
- 🔧 **Admin Dashboard:**
  - ✅ **Sensor Management** – Configure and monitor virtual air quality sensors.
  - ✅ **Alert Configuration** – Set thresholds for notifications.
  - ✅ **Simulation Control** – Manage and generate synthetic air quality data.

### 🛠️ Tech Stack
- **Backend:** C# ASP.NET Core
- **Database:** MySQL
- **Frontend:** HTML, CSS, JavaScript, Bootstrap
- **Mapping:** Leaflet.js

### ⚙️ Setup & Installation
#### 🔹 Prerequisites
- .NET SDK
- MySQL Server
- Node.js (for frontend development)

#### 🔹 Installation Steps
1. Clone the repository:
   ```sh
   git clone https://github.com/MiyuruDilakshan/AirQualityDashboard.git
   cd AirQualityDashboard
   ```
2. Set up the database:
   ```sql
   CREATE DATABASE AirQualityDB;
   ```
3. Configure environment variables for the database connection.
4. Run the backend:
   ```sh
   dotnet run
   ```
5. Open the frontend in a browser or use a local server.

### 📌 Usage
- **Public Users:** View real-time air quality data and alerts.
- **Monitoring Admins:** Monitor,analyze air quality trends,Manage sensors, alerts, and simulation settings
- **System Admins:** Manage sensors, alerts, and simulation settings and usermanagement.

### 🏗️ Future Improvements
- 🌍 **Live Data Integration** with IoT sensors
- 📡 **REST API for third-party applications**
- 📊 **Advanced data visualization**

### 📝 License
This project is open-source and available under the **MIT License**.

---
✨ Developed with passion for environmental awareness! 🌱
