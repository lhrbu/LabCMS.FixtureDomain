using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record RegisterEvent(
        string ProjectShortName,
        TestField TestField,
        string SetIndex,
        string? StorageInformation,
        string? AssetNo
        );
}
