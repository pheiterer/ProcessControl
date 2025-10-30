# Process Control Application

This project consists of a .NET backend API, an Angular frontend, and a PostgreSQL database, all orchestrated using Docker Compose.

## Prerequisites

Before you begin, ensure you have the following installed on your system:

*   **Docker Desktop:** Includes Docker Engine and Docker Compose. You can download it from [https://www.docker.com/products/docker-desktop](https://www.docker.com/products/docker-desktop).

## Getting Started

Follow these steps to get the application up and running using Docker Compose.

1.  **Navigate to the project root:**

    ```bash
    cd .\ProcessControl\
    ```

2.  **Build and start the services:**

    This command will build the Docker images for the frontend and backend, create the necessary containers, and start all services (database, backend, and frontend) in detached mode.

    ```bash
    docker-compose up -d --build
    ```

    The `--build` flag ensures that your images are rebuilt with the latest changes. If you've already built the images and only want to start the services, you can omit `--build`.

3.  **Access the Frontend:**

    Once all services are up, you can access the Angular frontend in your web browser at:

    [http://localhost:8080](http://localhost:8080)

    The backend API and PostgreSQL database are accessible internally within the Docker network and do not expose ports directly to your host machine.

## Stopping the Services

To stop and remove all containers, networks, and volumes created by `docker-compose up`, run the following command from the project root:

```bash
docker-compose down
```

If you also want to remove the anonymous volumes (like the PostgreSQL data volume), you can use:

```bash
docker-compose down -v
```
