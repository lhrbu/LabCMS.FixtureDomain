using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Services
{
    public interface IFixtureIndexGenerator
    {
        ValueTask<int> GetThenIncreaseAsync(int year);
    }
}
