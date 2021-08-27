using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record FixtureAssignLocationNoEvent:FixtureEvent
    {
        public FixtureAssignLocationNoEvent(int fixtureNo,int shelfNo,int floorNo):
            base(fixtureNo)
        {
            ShelfNo = shelfNo;
            FloorNo = floorNo;
        }
        public int ShelfNo { get; private set; }
        public int FloorNo {  get; private set; }  
        public override string TypeFullName { get ; protected set; } = 
            typeof(FixtureAssignLocationNoEvent).FullName!;
    }
}
