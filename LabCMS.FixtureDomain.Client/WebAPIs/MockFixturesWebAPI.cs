using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Client.WebAPIs
{
    public class MockFixturesWebAPI : IFixturesWebAPI
    {
        public ValueTask<IEnumerable<Fixture>> GetAllAsync()
        {
            Fixture[] fixtures = new[]
            {
                new Fixture(),
                new Fixture()
            };
            return ValueTask.FromResult(fixtures.AsEnumerable());
        }
    }
}
