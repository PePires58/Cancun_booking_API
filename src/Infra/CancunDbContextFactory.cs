using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra
{
    public class CancunDbContextFactory
        : IDesignTimeDbContextFactory<CancunDbContext>
    {
        public CancunDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<CancunDbContext>();

            optionsBuilder.UseMySQL(
                "Server=localhost;Database=cancun_db;User Id=cancun;Password=password"
            );

            return new CancunDbContext(optionsBuilder.Options);
        }
    }
}
