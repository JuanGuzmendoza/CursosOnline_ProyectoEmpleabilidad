# CursosOnlineFront

This is the frontend application for the CursosOnline platform, built with React, TypeScript, and Vite.

## Tech Stack

-   **Framework:** [React](https://react.dev/)
-   **Language:** [TypeScript](https://www.typescriptlang.org/)
-   **Build Tool:** [Vite](https://vitejs.dev/)
-   **Styling:** CSS (Vanilla/Modules)
-   **HTTP Client:** [Axios](https://axios-http.com/)
-   **Icons:** [Lucide React](https://lucide.dev/)
-   **Routing:** [React Router](https://reactrouter.com/)

## Prerequisites

Before you begin, ensure you have met the following requirements:

-   **Node.js**: v18 or higher
-   **npm**: v9 or higher
-   **Docker** (optional, for containerized deployment)

## Installation

1.  Clone the repository:

    ```bash
    git clone <repository-url>
    cd CursosOnlineFront
    ```

2.  Install dependencies:

    ```bash
    npm install
    ```

## Running Locally

To start the development server:

```bash
npm run dev
```

The application will be available at `http://localhost:5173` (or the port shown in your terminal).

## Building for Production

To build the application for production:

```bash
npm run build
```

The build artifacts will be stored in the `dist/` directory.

## Docker

### Build the Docker Image

To build the Docker image for this project, run:

```bash
docker build -t cursos-online-front .
```

### Run the Docker Container

To run the container and map port 80 of the container to port 8080 on your host machine:

```bash
docker run -d -p 8080:80 --name cursos-front cursos-online-front
```

Access the application at `http://localhost:8080`.
