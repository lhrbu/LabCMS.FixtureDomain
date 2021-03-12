using System;
using System.Threading.Tasks;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;
using LabCMS.Seedwork.PersonnelDomain;

namespace LabCMS.FixtureDomain.Server.Services
{
    public class FixtureStorageRecordService:IFixtureStorageRecordService
    {
        private readonly Repository _repository;
        public FixtureStorageRecordService(Repository repository)
        { _repository = repository; }
        public async ValueTask CheckoutAsync(Fixture fixture,CheckoutRecord checkoutRecord)
        {
            Person person = await _repository.People.FindAsync(checkoutRecord.ApplicantUserId);

            fixture.StorageInformation = $"Checkout by {person.Email} at {DateTimeOffset.Now.LocalDateTime}, Checkout record Id: {checkoutRecord.Id}";
        }
        public ValueTask CheckinAsync(Fixture fixture,CheckinRecord checkinRecord)
        {
            fixture.StorageInformation = $"Fixture Room";
            return ValueTask.CompletedTask;
        }
    }
}