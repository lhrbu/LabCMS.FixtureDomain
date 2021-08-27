
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureMoveInOutOfFixtureRoomEventHandler : FixtureEventHandler
{
    public FixtureMoveInOutOfFixtureRoomEventHandler(FixtureDomainRepository repository)
        :base(repository){}
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {
        if(fixtureEvent is FixtureMoveInOutOfFixtureRoomEvent moveInOutOfEvent)
        {
            Fixture fixture = await FindFixtureAsync(fixtureEvent);
            fixture.StorageInformation = moveInOutOfEvent.MoveIn ? "Fixture Room" : null;
            fixture.Status = moveInOutOfEvent.MoveIn ? FixtureStatus.FixtureRoom : FixtureStatus.Unknown;
            await AddFixtureEventIntoDatabaseAsync(moveInOutOfEvent);
            _repository.Update(fixture);
            await _repository.SaveChangesAsync();
        }
        else{ RaiseInvalidFixtureEventKind();}
    }
}
