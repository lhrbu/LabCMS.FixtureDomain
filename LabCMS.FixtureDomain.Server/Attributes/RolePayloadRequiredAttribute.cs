using LabCMS.FixtureDomain.Shared.ClientSideModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Attributes
{
    public class RolePayloadRequiredAttribute:Attribute
    {
        public string CookieName { get; } = nameof(RolePayload);
        public string HttpContextItemName { get; } = nameof(RolePayload);
    }
}
