using AutoMapper;
using LabCMS.FixtureDomain.Shared.ClientSideModels;
using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CheckoutRecordPayload, CheckoutRecord>();
            CreateMap<CheckinRecordPayload, CheckinRecord>();
            CreateMap<RolePayload,Role>();
            CreateMap<Role,RolePayload>();
        }
    }
}
