using Microsoft.EntityFrameworkCore;
using Zadanie_rekrutacyjne;
using Zadanie_rekrutacyjne.Database;
using Zadanie_rekrutacyjne.Interfaces;
using Zadanie_rekrutacyjne.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped<ITagsService, TagsService>();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
StackOverflowApiHelper.InitializeClient();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
