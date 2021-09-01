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
        public string UserId { get; init; } = null!;
        public string PasswordMD5MD5 { get; init; } = null!;
        public string Email { get; set; } = null!;
        public TestField[]? ResponsibleTestFields { get; init; }
    }
}
