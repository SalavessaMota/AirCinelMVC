# AirCinelMVC

## Description

**AirCinelMVC** is a web application for managing a fictitious airline. Developed with **ASP.NET MVC (.NET 5)**, it allows administrators to manage flights, airplanes, airports, and tickets, while also enabling customers to purchase and manage their tickets. This project was developed for the **CET Programming course at CINEL**.

## Features

- **User Management**: Registration and management of user accounts with different roles (Admin, Employee, Customer).
- **Flight Management**: Create, edit, and delete flights, linking them to airports and airplanes.
- **Ticket System**: Purchase tickets, view ticket details, and generate a boarding pass in PDF format.
- **Seat Management**: Choose seats through a dynamic seat map, with logic tailored to the airplane model.
- **Image Upload**: Upload and store profile pictures and other files using **Azure Blob Storage**.
- **Email Notifications**: Automatic emails for account confirmation and flight details.

## Technologies Used

- **ASP.NET MVC 5**
- **Entity Framework Core**
- **Azure Blob Storage**
- **Syncfusion (PDF Generation)**
- **Bootstrap 4.3.1** and **Materialize CSS**
- **JavaScript (AJAX, jQuery)**
- **Microsoft SQL Server** for database management

## Usage

Once the application is set up and running, you can access the following functionalities:

- **Home Page**: Customers can search for and purchase tickets.
- **Admin**: Access the control panel to manage airports, airplanes, flights, and users.
- **Employee**: Manage flights and consult all tickets.
- **Customer**: Manage profile, view tickets, and download the boarding pass in PDF format.

## Project Structure

- **Controllers/**: Handles the business logic of the application.
- **Models/**: Domain models representing the database entities.
- **Views/**: **Razor** views that define the graphical interface.
- **Helpers/**: Utility classes such as **ImageHelper** and **SeatHelper**, which handle image uploads and seat logic.
- **wwwroot/**: Contains static assets like images and CSS/JS files.

## Installation

1. Clone this repository:
   ```bash
   git clone https://github.com/salavessamota/aircinelmvc.git
 
