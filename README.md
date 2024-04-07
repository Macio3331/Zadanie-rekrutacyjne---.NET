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

The program downloads all needed data upon the first request and then uses it for all of the rest requests. To get the data user must speicfy the number of page (page=<number_of_page>, value by which tags should be sorted (sortBy=share or sortBy=name) and the order of sort (order=asc or order=desc). Share is passed as percentage value. Program contains Swagger that defines two possible endpoints - GET for acquiring information about tags and POST for triggering redownloading of tags. Application logs internet traffic in log file.

For now, program may be installed in the Docker container but unfortunatelly there is a problem with connection to the database with a container usage. Without Docker container program works fine.

Important!
Due to StackExchange's limitations, there is restricted number of possible connections with StackOverflow's API. Hence there is limited times application can be started (since it always fetches data at the first request after the startup) - this number is around 15 launchings.
