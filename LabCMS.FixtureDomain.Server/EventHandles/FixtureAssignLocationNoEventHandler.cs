
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureAssignLocationNoEventHandler : FixtureEventHandler
{
    public FixtureAssignLocationNoEventHandler(FixtureDomainRepository repository)
        : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {
        if(fixtureEvent is FixtureAssignLocationNoEvent assignLocationNoEvent)
        {
            Fixture fixture = await FindFixtureAsync(fixtureEvent);

            fixture.ShelfNo = assignLocationNoEvent.FixtureNo;
            fixture.FloorNo= assignLocationNoEvent.FloorNo;
            await AddFixtureEventIntoDatabaseAsync(assignLocationNoEvent);
            _repository.Fixtures.Update(fixture);
            await _repository.SaveChangesAsync();
        }
        else
        {
            RaiseInvalidFixtureEventKind();
        }
    }
}
