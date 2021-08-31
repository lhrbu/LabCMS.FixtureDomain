using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record FixtureCheckinScanEvent:FixtureEvent
    {
        public FixtureCheckinScanEvent(int fixtureNo) : base(fixtureNo) { }
        public DateTimeOffset CheckinDate { get; private set; } = DateTimeOffset.Now;
        public override string TypeFullName { get; protected set; } = 
            typeof(FixtureCheckinScanEvent).FullName!;
    }
}
