
using LabCMS.FixtureDomain.Server.Models;
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;
using Raccoon.Devkits.EmailToolkit;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureExternalCheckoutApplyEventHandler : FixtureEventHandler
{
    protected FixtureExternalCheckoutApplyEventHandler(
        FixtureDomainRepository repository) : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {

        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        if (fixture.CanCheckout())
        {
            fixture.Status = FixtureStatus.ExternalCheckoutApply;
            await EmailChannel.AddNotificationEmailAsync(
                "HNTC_Fixture@Hella.com", Repository.AdminRoles.Select(item => item.Email),
                 $"no-reply: Fixture{fixtureEvent.FixtureNo} need to checkout",
                 "<div>Check the link:</div><a href=\"http://10.99.159.149:83/\">link</a>");
    
        }
        else
        {
            throw new InvalidOperationException(
                $"Fixture No: ${fixtureEvent.FixtureNo} is in checkout procedure");
        }

    }
}
