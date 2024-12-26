# UrlShortener

**UrlShortener** is a web application designed to simplify URL management by providing an easy-to-use interface for creating short URLs. The app is composed of three distinct parts:
- An **ASP.NET Web API** backend for handling URL operations.
- An **Angular** client application for additional frontend functionality.
- A **Razor Pages** for modifiend URL shortening algorithm.

The entire application is containerized using Docker Compose for easy setup and deployment.

## Technologies Used
- **Backend**: ASP.NET Core Web API
- **Frontend**: Angular and Razor Pages (ASP.NET)
- **Database**: Microsoft SQL Server
- **Containerization**: Docker, Docker Compose
- **Hosting**: Runs on localhost via Docker

## Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop) installed on your machine.
- [.NET SDK](https://dotnet.microsoft.com/) (optional, for local development).

### Setup Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/UrlShortener.git
   cd UrlShortener
2. Start the application with Docker Compose:
   ```
   docker-compose up --build
   ```
3. Access the app:
    * Razor Pages UI: http://localhost:7047
    * Angular Frontend: http://localhost:5173
    * API: http://localhost:7006
## Project Structure
  ```
  UrlShortener/
  ├── UrlShortener.Api/          # ASP.NET Web API
  ├── UrlShortener.Client/       # Angular Frontend
  ├── UrlShortener.Algorithm/    # Razor Pages App
  ├── docker-compose.yml         # Docker Compose setup
  ```
## License
This project is licensed under the [MIT License](https://github.com/Artemiyqq/url-shortener/blob/master/LICENSE).
## Contributing
Feel free to fork the repository and submit pull requests. Suggestions and improvements are always welcome!
