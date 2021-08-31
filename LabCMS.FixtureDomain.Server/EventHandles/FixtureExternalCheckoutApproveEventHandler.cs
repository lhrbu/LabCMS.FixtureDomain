
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;
using Raccoon.Devkits.EmailToolkit;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureExternalCheckoutApproveEventHandler : FixtureEventHandler
{
    protected FixtureExternalCheckoutApproveEventHandler(FixtureDomainRepository repository)
        : base(repository) { }
    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {

        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        if (fixture.Status is FixtureStatus.ExternalCheckoutApply)
        {
            fixture.Status = FixtureStatus.ExternalCheckoutApprove;
            //Todo Send email notification here
            FixtureExternalCheckoutApplyEvent? applyEvent =
                Repository.FixtureEventsInDatabase.Where(item =>
                    item.FixtureNo == fixtureEvent.FixtureNo)
                .LastOrDefault(item => item.ContentTypeFullName ==
                    typeof(FixtureExternalCheckoutApplyEvent).FullName)?
                .GetEvent() as FixtureExternalCheckoutApplyEvent;
            try
            {
                Role? role = await Repository.Roles.FindAsync(applyEvent.ApplicantUserId);
                EmailSendService emailSendService = new("***", "***",
                    "***");
                _ = emailSendService.SendEmailAsync("", "", "", "");
            }
            catch { }
        }
    }
}
