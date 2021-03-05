using LabCMS.FixtureDomain.Shared;
using LabCMS.FixtureDomain.Shared.ClientSideModels;
using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Policies
{
    public class FixtureRoomAuthorizePolicy : IPermissionPolicy
    {
        public ValueTask<bool> ValidateAsync(RolePayload rolePayload, ICheckRecord checkRecord) =>
            ValueTask.FromResult(((int)rolePayload.AuthLevel >= (int)CheckRecordStatus.FixtureRoomApproved) &&
            (checkRecord.Status is CheckRecordStatus.TestRoomApproved));
    }
}
