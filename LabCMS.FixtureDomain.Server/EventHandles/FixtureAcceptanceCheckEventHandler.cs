
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureAcceptanceCheckEventHandler : FixtureEventHandler
{
    public FixtureAcceptanceCheckEventHandler(FixtureDomainRepository repository)
        : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {
        if(fixtureEvent is FixtureAcceptanceCheckEvent acceptanceCheckEvent)
        {
            
            Fixture fixture = await FindFixtureAsync(fixtureEvent);

            if(fixture!.Status != FixtureStatus.Registered)
            {
                throw new ArgumentException(
                    $"Fixture No: {fixtureEvent.FixtureNo} is not registered",nameof(fixtureEvent));
            }

            fixture.Status = FixtureStatus.AcceptanceChecked;
            fixture.StorageInformation = "已验收";
            await AddFixtureEventIntoDatabaseAsync(acceptanceCheckEvent);
            _repository.Update(fixture);
            await _repository.SaveChangesAsync();
        }
        else
        {
            RaiseInvalidFixtureEventKind();
        }
    }
}
