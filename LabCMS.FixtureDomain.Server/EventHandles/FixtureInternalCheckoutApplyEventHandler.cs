
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureInternalCheckoutApplyEventHandler : FixtureEventHandler
{
    protected FixtureInternalCheckoutApplyEventHandler(
        FixtureDomainRepository repository) : base(repository) { }

    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {

        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        if (fixture.CanCheckout())
        {
            fixture.Status = FixtureStatus.InternalCheckoutApply;
        }
        else {
            throw new InvalidOperationException(
              $"Fixture No: ${fixtureEvent.FixtureNo} is in checkout procedure");
        }
    }
}
