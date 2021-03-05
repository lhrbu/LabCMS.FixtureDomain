using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Models
{
    public record YearUsedIndex
    {
        [Key]
        public int Year { get; set; }
        public int UsedIndex { get; set; }
    }
}
