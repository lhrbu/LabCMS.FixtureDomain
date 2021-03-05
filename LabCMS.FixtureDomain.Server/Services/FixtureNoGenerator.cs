using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Services
{
    public class FixtureNoGenerator : IFixtureNoGenerator
    {
        public int Create(int index, Fixture fixture)
        {
            int typeFlag = fixture.TestFieldName.First() switch
            {
                'V'=>1,
                'E'=>2,
                'P'=>3,
                'W' or 'B' or 'F' or 'L' or 'S'=>4,
                'C' => 5,
                _=> 6
            };
            string yearFlag = DateTimeOffset.Now.LocalDateTime.Year.ToString().Substring(2,2);
            string indexFlag = index.ToString("D4");
            return int.Parse($"{typeFlag}0{yearFlag}{indexFlag}");
        }
    }
}
