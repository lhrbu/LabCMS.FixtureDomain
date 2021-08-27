using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record FixtureMoveInOutOfFixtureRoomEvent : FixtureEvent
    {
        public FixtureMoveInOutOfFixtureRoomEvent(int fixtureNo,bool moveIn=true)
            : base(fixtureNo) { MoveIn = moveIn; }
        public bool MoveIn { get;private set;  }
        public override string TypeFullName { get; protected set; }
            = typeof(FixtureMoveInOutOfFixtureRoomEvent).FullName!;
    }
}
