using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record FixtureAcceptanceCheckEvent:FixtureEvent
    {
        public FixtureAcceptanceCheckEvent(int fixtureNo) : base(fixtureNo) { }
        public override string TypeFullName { get; protected set; } =
            typeof(FixtureAcceptanceCheckEvent).FullName!;
    }
}
