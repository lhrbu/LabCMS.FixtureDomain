using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public record FixtureInternalCheckoutApplyEvent : FixtureEvent
    {
        public FixtureInternalCheckoutApplyEvent(
            int fixtureNo, string applicantUserId, string receiver,
            string receivePlace, string? comment) : base(fixtureNo)
        {
            ApplicantUserId = applicantUserId;
            Receiver = receiver;
            ReceivePlace = receivePlace;
            Comment = comment;
        }
        public string? ApplicantUserId { get; set; }
        public string Receiver { get; private set; }
        public string ReceivePlace { get; private set; }
        public DateTimeOffset CheckoutDate { get; private set; } = DateTimeOffset.Now;
        public DateTimeOffset PlannedReturnTime { get; private set; } = DateTimeOffset.Now;
        public string? Comment { get; private set; }
        public override string TypeFullName { get; protected set; } =
            typeof(FixtureInternalCheckoutApplyEvent).FullName!;
    }
}
