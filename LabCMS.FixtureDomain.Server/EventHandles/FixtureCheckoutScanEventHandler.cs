
using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.FixtureDomain.Shared.Events;
using LabCMS.FixtureDomain.Shared.Models;

namespace LabCMS.FixtureDomain.Server.EventHandles;
public class FixtureCheckoutScanEventHandler : FixtureEventHandler
{
    protected FixtureCheckoutScanEventHandler(FixtureDomainRepository repository)
        : base(repository) { }

    public override async ValueTask HandleAsync(FixtureEvent fixtureEvent)
    {

        Fixture fixture = await FindFixtureAsync(fixtureEvent);
        if (fixture.Status is FixtureStatus.ExternalCheckoutApprove or
            FixtureStatus.InternalCheckoutApply)
        {
            fixture.Status = FixtureStatus.CheckedOut;
            FixtureEventInDatabase? applyEventInDatabase = Repository.FixtureEventsInDatabase
                .LastOrDefault(item =>
                    item.ContentTypeFullName == typeof(FixtureExternalCheckoutApplyEvent).FullName ||
                    item.ContentTypeFullName == typeof(FixtureInternalCheckoutApplyEvent).FullName);
            FixtureEvent? applyEvent = applyEventInDatabase?.GetEvent();

            string? applicantUserId = null;
            if (applyEvent is FixtureInternalCheckoutApplyEvent internalCheckoutApplyEvent)
            {
                applicantUserId = internalCheckoutApplyEvent.ApplicantUserId;
            }
            else if (applyEvent is FixtureExternalCheckoutApplyEvent externalCheckoutApplyEvent)
            {
                applicantUserId = externalCheckoutApplyEvent.ApplicantUserId;
            }

            fixture.StorageInformation =
                $"Fixture No:${fixtureEvent.FixtureNo} is checked out by ${applicantUserId} at {DateTimeOffset.Now}";

        }
        else { throw new InvalidOperationException($"Fixture No: ${fixtureEvent.FixtureNo} is not in waiting for Checkout scan status."); }
    }
}

