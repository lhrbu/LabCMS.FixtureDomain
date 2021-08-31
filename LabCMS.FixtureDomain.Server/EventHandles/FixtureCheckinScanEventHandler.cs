
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureCheckinScanEventHandler:FixtureEventHandler
{
    protected FixtureCheckinScanEventHandler(
        FixtureDomainRepository repository) : base(repository) { }

    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {
        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        if(fixture.Status is FixtureStatus.CheckedOut)
        {
            fixture.Status = FixtureStatus.FixtureRoom;
            fixture.StorageInformation = "Fixture Room";
        }
    }
}
