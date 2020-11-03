using BowlingApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BowlingApi.Tests.RepositoryTest
{
    public class InMemoryDbContextFactory
    {
        private string _dbName;
        private BowlingDBContext _context;

        public InMemoryDbContextFactory()
        {
            _dbName = $"InMemoryBowlingDatabase_{Guid.NewGuid()}";

            var options = new DbContextOptionsBuilder<BowlingDBContext>()
                            .UseInMemoryDatabase(databaseName: _dbName)
                            .Options;
            _context = new BowlingDBContext(options);
        }

        public BowlingDBContext GetBowlingDbContext()
        {
            return _context;
        }
    }
}
