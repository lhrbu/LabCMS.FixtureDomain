using System.ComponentModel.DataAnnotations;

namespace LabCMS.FixtureDomain.Shared.ClientSideModels
{
    public record RolePayload
    {
        public string UserId { get; init; } = null!;
        public int AuthLevel { get; init; }
        public string? ResponseTestFieldName { get; set; }
    }
}