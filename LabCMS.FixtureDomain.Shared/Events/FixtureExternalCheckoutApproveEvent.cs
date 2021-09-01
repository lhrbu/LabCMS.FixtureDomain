using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record FixtureExternalCheckoutApproveEvent:FixtureEvent
    {
        public FixtureExternalCheckoutApproveEvent(
            int fixtureNo, string approverUserId):base(fixtureNo)
        {
            ApproverUserId = approverUserId;
        }
        public string? ApproverUserId { get; set; }
        public override string TypeFullName { get; protected set; } = 
            typeof(FixtureExternalCheckoutApproveEvent).FullName!;
    }
}
