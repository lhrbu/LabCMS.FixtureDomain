using LabCMS.FixtureDomain.Server.Repositories;
using LabCMS.Seedwork;
using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Server.Models;

namespace LabCMS.FixtureDomain.Server.Policies
{
    public class TestFieldResponsibleAuthorizePolicy : IPermissionPolicy
    {
        private readonly Repository _repository;
        public TestFieldResponsibleAuthorizePolicy(Repository repository) => _repository = repository;

        public async ValueTask<bool> ValidateAsync(RolePayload rolePayload, ICheckRecord checkRecord) =>
            ((int)rolePayload.AuthLevel >= (int)CheckRecordStatus.TestRoomApproved) &&
            (checkRecord.Status is CheckRecordStatus.Initial) &&
            (await ValidateTestFieldResponse(rolePayload, (checkRecord as CheckoutRecord)!));

        private async ValueTask<bool> ValidateTestFieldResponse(RolePayload rolePayload, CheckoutRecord checkoutRecord)
        {
            await _repository.Entry(checkoutRecord).Reference(item => item.Fixture).LoadAsync();
            return rolePayload.ResponseTestFieldName == checkoutRecord.Fixture!.TestFieldName;
        }
    }
}
