
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureExternalCheckoutApplyEventHandler : FixtureEventHandler
{
    public FixtureExternalCheckoutApplyEventHandler(
        FixtureDomainRepository repository) : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {
        if(fixtureEvent is FixtureExternalCheckoutApplyEvent applyEvent)
        {
            Fixture fixture =await FindFixtureAsync(fixtureEvent);
            if(fixture.Status != FixtureStatus.ExternalCheckoutApply &&
                fixture.Status != FixtureStatus.ExternalCheckoutApprove)
            {
                fixture.Status = FixtureStatus.ExternalCheckoutApply;
                await AddFixtureEventIntoDatabaseAsync(applyEvent);
                _repository.Update(fixture);
                await _repository.SaveChangesAsync();

                //Todo Send email notification here
            }
            else
            {
                throw new InvalidOperationException(
                    $"Fixture No: ${fixtureEvent.FixtureNo} is in external checkout procedure");
            }
        }
        else { RaiseInvalidFixtureEventKind(); }
    }
}
