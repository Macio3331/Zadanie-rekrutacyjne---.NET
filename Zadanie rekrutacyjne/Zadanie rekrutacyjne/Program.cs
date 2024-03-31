using Microsoft.EntityFrameworkCore;
using Zadanie_rekrutacyjne;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Services;

/*void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    if (scopedFactory != null)
    {
        using (var scope = scopedFactory.CreateScope())
        {
            if (scope != null)
            {
                scope.ServiceProvider.GetService<TagsSeeder>().Seed();
            }
        }
    }
}*/

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<ITagsService, TagsService>();
/*builder.Services.AddScoped<TagsSeeder>();*/
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
StackOverflowApiHelper.InitializeClient();
/*SeedData(app);*/

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
