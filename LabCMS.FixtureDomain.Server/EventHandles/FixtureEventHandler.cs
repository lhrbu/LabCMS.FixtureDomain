using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public abstract class FixtureEventHandler
{
    protected readonly FixtureDomainRepository _repository;
    public FixtureEventHandler(FixtureDomainRepository repository)
    { _repository = repository; }
    protected async ValueTask<Fixture> FindFixtureAsync(FixtureEvent fixtureEvent)
    {
        Fixture? fixture = await _repository.Fixtures.FindAsync(fixtureEvent.FixtureNo);
        return fixture??throw new InvalidOperationException(
            $"Fixture No: {fixtureEvent.FixtureNo} doesn't exist");
    }

    protected void RaiseInvalidFixtureEventKind()
    {
        throw new ArgumentException($"Ivalid kind of fixture event is given");
    }

    protected async ValueTask AddFixtureEventIntoDatabaseAsync(FixtureEvent fixtureEvent)
    {
        await _repository.FixtureEventsInDatabase.AddAsync(
            FixtureEventInDatabase.GetEntity(fixtureEvent));
    }
    public abstract ValueTask HandleAsync(FixtureEvent fixtureEvent);
}
