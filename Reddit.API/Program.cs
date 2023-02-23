using Microsoft.Extensions.Caching.Memory;
using Reddit.Core;
using Reddit.Core.Interfaces;
using Reddit.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//var redditConfig = builder.Configuration.GetSection("RedditConfig").Get<RedditConfig>();

builder.Services.Configure<RedditConfig>(
    builder.Configuration.GetSection("RedditConfig"));

builder.Services.AddTransient<IMemoryCache, MemoryCache>();


builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IAuthorizeService, AuthorizeService>();

builder.Services.AddHttpClient();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
