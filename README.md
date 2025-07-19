# HeavyStringFiltering

This repository implements a system for filtering large text files by splitting them into chunks, uploading the chunks, assembling them, and then filtering the assembled text using a Levenshtein distance-based filter.  The filtered text is then saved for later retrieval.

## Features and Functionality

*   **Chunked Upload:**  Accepts large text input as a series of smaller chunks via an API endpoint.
*   **Asynchronous Processing:**  Assembles the text from chunks and performs filtering in a background service using a queue.
*   **Levenshtein Distance Filtering:**  Removes words from the text that are similar to a predefined list of filter words, based on a configurable similarity threshold.
*   **In-Memory Storage:** Uses in-memory storage for chunk storage, queue, and results for simplicity and speed.  (Suitable for development and testing; production deployments would require persistent storage.)
*   **Health Checks:** Provides a `/health` endpoint for monitoring service availability.
*   **Metrics:** Exposes metrics via Prometheus for monitoring performance.
*   **Global Exception Handling:** Implements global exception handling using `GlobalExceptionHandler.cs`

## Technology Stack

*   **C#:** Primary programming language.
*   **.NET 7:** Target framework.
*   **ASP.NET Core:** Web API framework.
*   **MediatR:** In-process messaging library for handling commands and queries.
*   **FluentValidation:** Validation library for data input validation.
*   **Bogus:** Library for generating fake data (used in tests).
*   **Moq:** Mocking library for unit testing.
*   **Microsoft.Extensions.DependencyInjection:**  Dependency injection container.
*   **Microsoft.Extensions.Logging:** Logging framework.
*   **Prometheus:** Monitoring and alerting toolkit.

## Prerequisites

*   .NET 7 SDK installed.
*   An IDE or text editor for code modification (e.g., Visual Studio, VS Code, Rider).
*   (Optional) Prometheus and Grafana for monitoring.

## Installation Instructions

1.  **Clone the repository:**

    ```bash
    git clone https://github.com/ismayil767/HeavyStingFiiltering.git
    cd HeavyStingFiiltering
    ```

2.  **Restore NuGet packages:**

    ```bash
    dotnet restore
    ```

3.  **Build the project:**

    ```bash
    dotnet build
    ```

## Usage Guide

1.  **Configure `FilterSettings`:**

    Edit the `appsettings.json` file to configure the `FilterSettings` section. This section defines the list of words to filter and the similarity threshold.

    ```json
    {
      "FilterSettings": {
        "FilterWords": [
          "apple",
          "banana",
          "orange"
        ],
        "SimilarityThreshold": 0.8
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*"
    }
    ```

    *   `FilterWords`: A list of words that will be used as the basis for filtering similar words from the input text.
    *   `SimilarityThreshold`: A value between 0 and 1, representing the minimum similarity score for a word to be considered similar to a filter word and removed.

2.  **Run the application:**

    ```bash
    dotnet run --project HeavyStringFiltering/HeavyStringFiltering.csproj
    ```

3.  **Upload chunks via the API:**

    Send a `POST` request to the `api/Upload/upload` endpoint with a JSON payload representing an `UploadChunkCommand`.

    Example request using `curl`:

    ```bash
    curl -X POST \
      http://localhost:5000/api/Upload/upload \
      -H 'Content-Type: application/json' \
      -d '{
            "uploadId": "unique-upload-id",
            "chunkIndex": 0,
            "data": "This is the first chunk of text.",
            "isLastChunk": false
          }'
    ```

    *   `uploadId`: A unique identifier for the upload.  Must be the same for all chunks of a single text.
    *   `chunkIndex`: The index of the chunk, starting from 0.  Chunks must be uploaded in order.
    *   `data`: The text data of the chunk.
    *   `isLastChunk`: A boolean indicating whether this is the last chunk of the upload.  Set to `true` for the final chunk.

    Example of the last chunk:

     ```bash
    curl -X POST \
      http://localhost:5000/api/Upload/upload \
      -H 'Content-Type: application/json' \
      -d '{
            "uploadId": "unique-upload-id",
            "chunkIndex": 1,
            "data": " This is the second and final chunk.",
            "isLastChunk": true
          }'
    ```

4.  **Retrieve the filtered result:**

    *   The application doesn't currently have an API to retrieve the result. This feature would require implementing a `ResultController` with an endpoint to query the `IResultStore` (specifically, the `InMemoryResultStore`).  This functionality is left as an exercise for the user.  You would typically create a GET endpoint accepting the `uploadId` and returning the filtered text, or an error if the `uploadId` is not found.  You can inspect the `InMemoryResultStore` directly in debugging for testing purposes.

5.  **Monitor with Prometheus (Optional):**

    *   Configure Prometheus to scrape the `/metrics` endpoint.
    *   Access the Prometheus UI or Grafana to view the collected metrics.

## API Documentation

### `POST /api/Upload/upload`

Uploads a chunk of text data.

**Request Body:**

```json
{
  "uploadId": "string",
  "chunkIndex": 0,
  "data": "string",
  "isLastChunk": true
}
```

**Response Body:**

```json
{
  "status": "Accepted" // or "Failed"
}
```

## Contributing Guidelines

1.  Fork the repository.
2.  Create a new branch for your feature or bug fix.
3.  Implement your changes.
4.  Write unit tests to cover your changes.  See `HeavyStringFiltering.Application.Test` and `HeavyStringFiltering.Infrastructure.Test` for examples.
5.  Submit a pull request.

## License Information

No license has been specified for this project.  All rights are reserved by the author.

## Contact/Support Information

For questions or support, please contact ismayil767@example.com (replace with the actual contact information).