using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Models
{
    public record FixtureRegisterPayload(
        string ProjectShortName,
        TestField TestField,
        string SetIndex);
}
