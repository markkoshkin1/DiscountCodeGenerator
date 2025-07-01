# Discount Generator

A full-stack web application for generating and managing promotional discounts. This project was created as part of an interview process.

## Table of Contents

- [About The Project](#about-the-project)
- [Tech Stack](#tech-stack)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Running the Application](#running-the-application)
- [Usage](#usage)
- [Project Structure](#project-structure)

## About The Project

[Provide a more detailed description of the project here. What problem does it solve? What are the main features?]

Key Features:
*   Generate unique discount codes.
*   Use discount codes.

## Tech Stack

This project is built with the following technologies:

**Client app:**

**Frontend:**
*   [React](https://reactjs.org/)
*   [Vite](https://vitejs.dev/)
*   [Material-ui](https://mui.com/material-ui/)

**Backend:**
*   [ASP.NET Core Web API](https://learn.microsoft.com/en-us/aspnet) 

**Service:**
* [ASP.NET Grpc](https://learn.microsoft.com/en-us/aspnet)
* [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server)


## Getting Started

To get a local copy up and running, follow these simple steps.

### Prerequisites

Make sure you have the following software installed on your machine:
*   [Git](https://git-scm.com/)
*   [Node.js](https://nodejs.org/) (which includes npm)
*   [.NET SDK](https://dotnet.microsoft.com/download) (if using ASP.NET Core)
*   [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server)

### Installation

1.  **Clone the repository**
    ```sh
    git clone https://github.com/your-username/DiscountGenerator.git
    cd DiscountGenerator
    ```

2.  **Set up the Service**
    *(Backend located in a `backend\DiscountCodeGenerator` directory.)*

	```sh
    You may need to setup database connection strings in `appsettings.Development.json`
	Change logs folder destination
    ```

3.  **Set up the Frontend**
    ```sh
    Execute via visual studio
    You may need to adjust urls
    ```

### Running the Application
1.  **Start service**
    *(From the `backend\DiscountCodeGenerator` directory)*
    Use cli or execute via Visual studio

2.  **Start the Client app**
    *(From the `DiscountGenerator\frontend\ReactClient` directory)*
    Use cli or execute via Visual studio

## Usage

Once the application is running, open your browser to the frontend URL.

1.  Fill out the form to create a new discount.
2.  Use generated discount code.


## Project Structure

The project is organized into two main parts: frontend and backend.

```
DiscountGenerator/
├── backend/                  # Contains the backend solution and projects
│   ├── DiscountGenerator.sln
│   └── ...
└── frontend/
    └── ReactClient/
        └── reactclient.client/   # Contains the React frontend application
            ├── public/
            ├── src/
            ├── package.json
            └── ...
```