using LabCMS.FixtureDomain.Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Attributes
{
    public class RolePayloadRequiredAttribute:Attribute
    {
        public bool Required { get; }
        public RolePayloadRequiredAttribute(bool required=true)
        {
            Required = required;
        }
    }
}
