using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public abstract class FixtureEventHandler
{
    public FixtureDomainRepository Repository { get; } = null!;
    protected FixtureEventHandler(FixtureDomainRepository repository)
    { Repository = repository; }
    protected async ValueTask<Fixture> FindFixtureAsync(FixtureEvent fixtureEvent)
    {
        Fixture? fixture = await Repository.Fixtures.FindAsync(fixtureEvent.FixtureNo);
        return fixture??throw new InvalidOperationException(
            $"Fixture No: {fixtureEvent.FixtureNo} doesn't exist");
    }

    public abstract ValueTask HandleAsync(FixtureEvent fixtureEvent);
}
