using System;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.InMemory;
using Primal;
using Primal.Repositories;
using Primal.Entities;
using Primal.Business;

namespace PrimalTests
{
    public class Tests
    {
        private readonly DatabaseContext dbContext;

        public Tests()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string>())
                .Build();
            var builder = new DbContextOptionsBuilder<DatabaseContext>();
        }
    }
}
