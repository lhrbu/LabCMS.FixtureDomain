
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

            try
            {
                EmailSendService emailSendService = new("***", "***",
                     "***");
                _ = emailSendService.SendEmailAsync("", "", "", "");
            }
            catch { }
        }
        else
        {
            throw new InvalidOperationException(
                $"Fixture No: ${fixtureEvent.FixtureNo} is in external checkout procedure");
        }

    }
}
