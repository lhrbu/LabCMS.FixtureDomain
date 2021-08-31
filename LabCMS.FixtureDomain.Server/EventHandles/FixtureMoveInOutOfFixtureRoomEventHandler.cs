
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureMoveInOutOfFixtureRoomEventHandler : FixtureEventHandler
{
    protected FixtureMoveInOutOfFixtureRoomEventHandler(FixtureDomainRepository repository)
        : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {

        FixtureMoveInOutOfFixtureRoomEvent moveInOutOfEvent = (fixtureEvent
            as FixtureMoveInOutOfFixtureRoomEvent)!;
        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        fixture.StorageInformation = moveInOutOfEvent.MoveIn ? "Fixture Room" : null;
        fixture.Status = moveInOutOfEvent.MoveIn ? FixtureStatus.FixtureRoom : FixtureStatus.Unknown;
    }
}
