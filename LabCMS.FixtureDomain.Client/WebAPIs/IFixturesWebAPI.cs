using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Client.WebAPIs
{
    public interface IFixturesWebAPI
    {
        ValueTask<IEnumerable<Fixture>> GetAllAsync();
    }
}
