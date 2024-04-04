# Zadanie rekrutacyjne - .NET
Exercise:
Prepare the REST API in .NET 8 and C#, which internally will be based on a list of tags provided by API StackOverflow (https://api.stackexchange.com/docs).
Main requirements:

Downloading can occur at startup or upon the first request, either entirely at once or gradually, fetching only the missing data.
Calculate the percentage share of tags in the entire downloaded population (source field count, appropriately converted).
Provide tags via paginated API with sorting options by name and share in both directions.
Offer an API method to trigger re-download of tags from SO.
Provide the OpenAPI definition of the prepared API methods.
Include logging, error handling, and service runtime configuration.
Prepare several selected internal service unit tests.
Prepare several selected integration tests based on the provided API.
Utilize containerization to ensure repeatable project building and execution.
Publish the solution in a GitHub repository.
The entire system should be runnable solely by executing the command 'docker compose up'.
