using MPWebAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace MPWebAPI.Fixtures
{
    public static class FixtureBuilder
    {
        public static void AddFixture(this IApplicationBuilder app, string fixtureFile)
        {
            var db = app.ApplicationServices.GetService<PostgresDBContext>();
            db.Database.EnsureDeleted();
            db.Database.Migrate();
            
            // Parse migration json

            // Create db objects

            // Save the objects
        }
    }
}