
using LabCMS.FixtureDomain.Server.Models;
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
            if(applyEvent == null)
            { throw new InvalidOperationException(
                $"Can't find apply event for this approve event! Fixture No:{fixtureEvent.FixtureNo}");}
            Role? role = (await Repository.Roles.FindAsync(applyEvent?.ApplicantUserId));
            if(role == null)
            { throw new InvalidOperationException($"Applicant: {applyEvent.ApplicantUserId} doesn't exist!");}
            

            await EmailChannel.AddNotificationEmailAsync(
            "HNTC_Fixture@Hella.com", new[] { role.Email },
            $"no-reply: Fixture {fixtureEvent.FixtureNo} checkout approved",
            "<div>Check the link:</div><a href=\"http://10.99.159.149:83/\">link</a>");
        }
        else { throw new InvalidOperationException($"Fixture No: ${fixtureEvent.FixtureNo} is not in Apply status."); }
    }
}
