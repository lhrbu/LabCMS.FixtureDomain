using LabCMS.FixtureDomain.Server.Models;
using LabCMS.FixtureDomain.Server.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace LabCMS.FixtureDomain.Server.Services
{
    public class DatabaseFixtureIndexGenerator : IFixtureIndexGenerator
    {
        private readonly FixtureYearUsedIndicesRepository _repository;
        public DatabaseFixtureIndexGenerator(FixtureYearUsedIndicesRepository repository)
        { _repository = repository; }

        public async ValueTask<int> GetThenIncreaseAsync(int year)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel=IsolationLevel.Serializable},
                TransactionScopeAsyncFlowOption.Enabled);
            
            YearUsedIndex? yearUsedIndex = await _repository.YearUsedIndices.FindAsync(year);
            if(yearUsedIndex is not null)
            {
                int result = ++yearUsedIndex.UsedIndex;
                _repository.Update(yearUsedIndex);
                await _repository.SaveChangesAsync();
                scope.Complete();
                return result;
            }
            else
            {
                yearUsedIndex = new() { Year = year, UsedIndex = 0 };
                await _repository.AddAsync(yearUsedIndex);
                await _repository.SaveChangesAsync();
                scope.Complete();
                return 1;
            }
        }
    }
}
