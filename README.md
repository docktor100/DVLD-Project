# Drivers & Vehicles Licenses Department (DVLD)

## 📘 About the Project

This is an **educational project** developed independently using **C# Windows Forms** on the **.NET Framework**. The **UI design and database** were taken from the learning platform, while all logic and implementation were built from scratch.

The application simulates a real-world **Drivers and Vehicles Licenses Department**, providing a range of features for managing applicants, users, and license-related services. to see the **project showcase** click [here](https://www.linkedin.com/posts/abullah-akram-al-jaf-a23b84379_a-showcase-of-my-dvld-project-took-me-about-activity-7362088246698205184-abJ_?utm_source=share&utm_medium=member_android&rcm=ACoAAF2cvLsBDSl8Xn3XYB8oUgyuAaJmbcFPF6M).

---

## 🔧 Core Functionalities

- Full management of people applying for licenses and system users  
- Handling various license applications, including:
  - New local license issuance (rank-based)
  - Replacement for lost or damaged licenses
  - License detainment and release
  - Global license issuance  
- All data and application actions are stored in an integrated SQL Server database  
- Every user action is tracked and saved for accountability

---

## 🗺️ Database Diagram

<img width="4855" height="3521" alt="DVLD Database" src="https://github.com/user-attachments/assets/b6c3fb04-b061-4085-b0d4-7997348b860a" />

---
## 🧪 Basic Walkthrough

1. **Add a new person to the system**  
   - Go to `People → Add New`.  
   - Fill in the details and click **Save**.  

2. **Apply for a new local driving license**  
   - Navigate to `Application Types → Driving License Services → New Driving License → Local Driving License`.  
   - Choose a person.  
   - From the second tab page, select the **license class**.  

3. **Take the required tests for license issuance**  
   - Go to `Application Types → Manage Applications → Local Driving License Applications`.  
   - Select the desired application, right-click, and choose **Schedule Tests**.  
   - Click the **Add Appointment** button in the upper-right corner of the data grid view.  
   - Choose the date and save the appointment.  
   - Right-click on the appointment and take the test.  
   - Repeat the process until all required tests are completed.  

4. **Issue the new local license**  
   - Right-click the application and choose `Issue License → Issue`.  


## **Additional features include:**

- Global license issuance  
- Replacement for lost or damaged licenses  
- License renewal  
- Detainment and release of licenses  

> Note: Each application has its own conditions (e.g., license must be active, not expired, etc.)

---

## 💡 Key Features

- Built using a **3-tier architecture** (Presentation, Business Logic, Data Access)  
- Reusability through **custom UserControls**  
- Fully integrated with the database using **ADO.NET**  
- Built with **strong and clean OOP principles**  
- Includes a basic **sign-in / sign-out system**

---

## ⚙️ Tech Stack

- **Language**: C#  
- **Framework**: .NET Framework (Windows Forms)  
- **Database**: SQL Server (T-SQL)  
- **Data Access**: ADO.NET

---

## ▶️ Getting Started

1. Clone the repository:  
   ```bash
   git clone https://github.com/docktor100/DVLD-Project.git

2. **Restore the database**   
   Restore the `DVLD.bak` database file (included in the `/Database` folder) to your local SQL Server instance, and make sure to name it "`DVLD`"

3. **Configure the connection string**   
   Open the `App.config` file and update the connection string with your SQL Server credentials:  
   ```xml
   <appSettings>
       <add key="DB_ConnectionString" value="Server=YOUR_SERVER_NAME;Database=DVLD;User Id=YOUR_USER;Password=YOUR_PASSWORD" />
   </appSettings>
4. **Open the solution**   
   Open the `.sln` file in Visual Studio 2022 or later.  
   Ensure the **.NET desktop development** workload is installed.  
   If not, open the Visual Studio Installer, choose **Modify**, and select the **.NET desktop development** workload.

5. **Log in**  
   Enter the `username` "Ali" and the `password` "1234", then select the "Remember Me" option.


## 📝 Note

This project was developed as part of a self-learning journey to practice and demonstrate real-world application structure, database connectivity, and desktop UI development. It’s not perfect and can definitely be further optimized and enhanced, but that’s part of the fun — there’s plenty of room for improvement and experimentation.
