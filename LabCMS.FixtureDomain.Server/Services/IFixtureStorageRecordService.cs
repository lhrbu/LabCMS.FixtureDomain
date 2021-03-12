using System;
using LabCMS.Seedwork.FixtureDomain;

namespace LabCMS.FixtureDomain.Server.Services
{
    public interface IFixtureStorageRecordService
    {
        void Checkout(Fixture fixture,CheckoutRecord checkoutRecord);
        void Checkin(Fixture fixture,CheckinRecord checkinRecord);
    }
}