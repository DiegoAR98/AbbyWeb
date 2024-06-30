using Abbyweb.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load .env file in development
if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.Load();
}

// Add services to the container.
builder.Services.AddRazorPages();

// Configure the database connection
string connectionString;

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    var databaseUri = new Uri(databaseUrl);
    var userInfo = databaseUri.UserInfo.Split(':');

    var npgsqlBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = databaseUri.Host,
        Port = databaseUri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = databaseUri.LocalPath.TrimStart('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    connectionString = npgsqlBuilder.ToString();
}
else
{
    throw new InvalidOperationException("The environment variable DATABASE_URL is not set.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
