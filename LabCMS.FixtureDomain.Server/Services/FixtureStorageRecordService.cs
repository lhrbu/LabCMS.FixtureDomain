using System;
using LabCMS.Seedwork.FixtureDomain;

namespace LabCMS.FixtureDomain.Server.Services
{
    public class FixtureStorageRecordService:IFixtureStorageRecordService
    {
        public void Checkout(Fixture fixture,CheckoutRecord checkoutRecord)
        {
            fixture.StorageInformation = $"Checkout by {checkoutRecord.ApplicantUserId}, Checkout record Id: {checkoutRecord.Id} at {DateTimeOffset.Now.LocalDateTime}";
        }
        public void Checkin(Fixture fixture,CheckinRecord checkinRecord)
        {
            fixture.StorageInformation = $"Fixture Room";
        }
    }
}