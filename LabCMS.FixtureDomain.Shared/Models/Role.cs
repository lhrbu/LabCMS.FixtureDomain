using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Models
{
    public record Role
    {
        [Key]
        public string Name { get; init; } = null!;
        public TestField[]? ResponsibleTestFields { get; init; }
    }
}
