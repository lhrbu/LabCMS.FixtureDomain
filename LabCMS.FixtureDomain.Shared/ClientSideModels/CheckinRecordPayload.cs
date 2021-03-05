using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.ClientSideModels
{
    public record CheckinRecordPayload
    {
        public int CheckoutRecordId {get;init;}
        public int FixtureNo { get; init; }
    }
}
