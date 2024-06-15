using System.Collections.Generic;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Primal.Entities;

namespace Primal
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) :
            base(options)
        {
        }
    }
}
