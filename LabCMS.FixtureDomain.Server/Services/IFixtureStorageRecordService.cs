using System;
using System.Threading.Tasks;
using LabCMS.Seedwork.FixtureDomain;

namespace LabCMS.FixtureDomain.Server.Services
{
    public interface IFixtureStorageRecordService
    {
        ValueTask CheckoutAsync(Fixture fixture,CheckoutRecord checkoutRecord);
        ValueTask CheckinAsync(Fixture fixture,CheckinRecord checkinRecord);
    }
}