using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;
using System;

namespace Abbyweb.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var environmentVariable = Environment.GetEnvironmentVariable("DATABASE_URL");
            if (string.IsNullOrEmpty(environmentVariable))
            {
                throw new InvalidOperationException("The environment variable DATABASE_URL is not set.");
            }

            var databaseUri = new Uri(environmentVariable);
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

            var connectionString = npgsqlBuilder.ToString();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
