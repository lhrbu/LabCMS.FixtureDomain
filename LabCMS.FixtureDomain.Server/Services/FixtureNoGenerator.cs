using LabCMS.FixtureDomain.Server.Models;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace LabCMS.FixtureDomain.Server.Services
{
    public class FixtureNoGenerator
    {
        private readonly FixtureYearUsedIndicesRepository _repository;
        public FixtureNoGenerator(FixtureYearUsedIndicesRepository repository)
        { _repository = repository; }
        public async ValueTask<int> CreateAsync(string testFieldName,int year=default)
        {
            int typeFlag = testFieldName.First() switch
            {
                'V'=>1,
                'E'=>2,
                'P'=>3,
                'W' or 'B' or 'F' or 'L' or 'S'=>4,
                'C' => 5,
                _=> 6
            };
            if (year == default) { year = DateTimeOffset.Now.LocalDateTime.Year; }
            string yearFlag = year.ToString().Substring(2,2);

            string indexFlag = (await GetThenIncreaseIndexAsync(year)).ToString("D4");
            return int.Parse($"{typeFlag}0{yearFlag}{indexFlag}");
        }

        public async ValueTask<int> GetThenIncreaseIndexAsync(int year)
        {
            using TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions { IsolationLevel = IsolationLevel.Serializable },
                TransactionScopeAsyncFlowOption.Enabled);
            YearUsedIndex? yearUsedIndex = await _repository.YearUsedIndices.FindAsync(year);
            if (yearUsedIndex is not null)
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
