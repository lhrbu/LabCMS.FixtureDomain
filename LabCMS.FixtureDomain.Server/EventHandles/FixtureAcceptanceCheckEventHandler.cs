
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureAcceptanceCheckEventHandler : FixtureEventHandler
{
    public FixtureAcceptanceCheckEventHandler() : base(null) { }
    protected FixtureAcceptanceCheckEventHandler(FixtureDomainRepository repository)
        : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {
        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        if (fixture!.Status != FixtureStatus.Registered)
        {
            throw new ArgumentException(
                      $"Fixture No: {fixtureEvent.FixtureNo} is not registered", nameof(fixtureEvent));
        }
        fixture.Status = FixtureStatus.AcceptanceChecked;
        fixture.StorageInformation = "已验收";

    }
}
