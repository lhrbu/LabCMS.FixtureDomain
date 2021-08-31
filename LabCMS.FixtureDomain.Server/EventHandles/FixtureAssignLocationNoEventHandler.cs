
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureAssignLocationNoEventHandler : FixtureEventHandler
{
    protected FixtureAssignLocationNoEventHandler(FixtureDomainRepository repository)
        : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {

        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        FixtureAssignLocationNoEvent assignLocationNoEvent = (fixtureEvent
            as FixtureAssignLocationNoEvent)!;
        fixture.ShelfNo = assignLocationNoEvent.FixtureNo;
        fixture.FloorNo = assignLocationNoEvent.FloorNo;
    }
}
