using LabCMS.FixtureDomain.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Repositories
{
    public class FixtureYearUsedIndicesRepository:DbContext
    {
        public FixtureYearUsedIndicesRepository(DbContextOptions<FixtureYearUsedIndicesRepository> options)
            : base(options) { }
        public DbSet<YearUsedIndex> YearUsedIndices => Set<YearUsedIndex>();
    }
}
