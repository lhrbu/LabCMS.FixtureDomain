using AutoMapper;
using LabCMS.Seedwork.FixtureDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Server.Models;

namespace LabCMS.FixtureDomain.Server.Profiles
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