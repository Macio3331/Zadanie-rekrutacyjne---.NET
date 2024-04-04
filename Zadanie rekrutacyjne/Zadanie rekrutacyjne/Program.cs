using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Zadanie_rekrutacyjne;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Models;
using Zadanie_rekrutacyjne.Repositories;
using Zadanie_rekrutacyjne.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure logging.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("app_logs.log", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = "Data Source = " + Environment.GetEnvironmentVariable("DB_HOST") + ";" +
    " Initial Catalog = " + Environment.GetEnvironmentVariable("DB_NAME") + ";" +
    " User ID = sa; Password = " + Environment.GetEnvironmentVariable("DB_SA_PASSWORD") + ";" +
    " Trust Server Certificate = True; Trusted_Connection = True; Integrated Security = True;";
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddScoped<ITagsRepository, TagsRepository>();
builder.Services.AddSingleton<WasLoadedModel>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddHttpClient<ITagsApiClient, TagsApiClient>(apiClient => { }).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler() { AutomaticDecompression = System.Net.DecompressionMethods.GZip});

// Add HealthChecks services.
builder.Services.AddHealthChecks();

// Configure controllers and Swagger.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Recruitment task",
        Description = "Program made for recruitment task that retrieves information about tags from StackOverflow API, feeds the database with them and returns them with some additional information.\n" +
        "Main requirements:\n" +
        " - Take at least 1000 tags and store it inside of local database or other persistant cache;\n" +
        " - Data can be fetched either at startup or upon the first request, either all at once or gradually, only the missing data;\n" +
        " - Calculate the percentage share of tags in the entire downloaded population (the source field count, properly converted);\n" +
        " - Provide tags through paginated API with sorting option by name and share in both directions;\n" +
        " - Provide an API method to force re-downloading of tags from StackOverflow;\n" +
        " - Provide the OpenAPI definition of the prepared API methods;\n" +
        " - Include logging, error handling, and runtime service configuration;\n" +
        " - Prepare a few selected internal service unit tests;\n" +
        " - Prepare a few selected integration tests based on the provided API;\n" +
        " - Use containerization to ensure repeatable building and running of the project;\n" +
        " - Publish the solution in a GitHub repository;\n" +
        " - The entire system should start running only by executing the command 'docker compose up'.",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Maciej Musia³",
            Email = "maciejmusial5422@gmail.com"
        }
    });
    var filename = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
    var filepath = Path.Combine(AppContext.BaseDirectory + filename);
    options.IncludeXmlComments(filepath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recruitment task v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks("/healthcheck");
app.MapControllers();

app.Run();

public partial class Program { }