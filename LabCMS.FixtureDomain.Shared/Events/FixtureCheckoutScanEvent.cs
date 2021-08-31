using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record FixtureCheckoutScanEvent:FixtureEvent
    {
        public FixtureCheckoutScanEvent(int fixtureNo)
            : base(fixtureNo) { }
        public override string TypeFullName { get; protected set; } =
            typeof(FixtureCheckoutScanEvent).FullName!;
    }
}
