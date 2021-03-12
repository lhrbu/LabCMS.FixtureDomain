using System;
using System.ComponentModel.DataAnnotations;

namespace LabCMS.FixtureDomain.Server.Models
{
    public record RolePayload
    {
        public string UserId { get; init; } = null!;
        public int AuthLevel { get; init; }
        public string? ResponseTestFieldName { get; set; }
    }
}