using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Server.Models;

namespace LabCMS.FixtureDomain.Server.Policies
{
    public class ApplicantOnlyPolicy : IPermissionPolicy
    {
        public ValueTask<bool> ValidateAsync(RolePayload rolePayload, ICheckRecord checkRecord) => 
           ValueTask.FromResult(rolePayload.UserId == checkRecord.ApplicantUserId);
    }
}
