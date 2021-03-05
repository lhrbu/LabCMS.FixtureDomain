using LabCMS.Seedwork.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.ClientSideModels
{
    public record CheckoutRecordPayload
    {
        public int FixtureNo { get; init; }
        public string ReceiverCompany { get; init; } = null!;
        public string Receiver { get; init; } = null!;
        [JsonConverter(typeof(JsonConverters.DateTimeOffsetJsonConverter))]
        public DateTimeOffset PlanndReturnDate { get; init; }
    }
}
