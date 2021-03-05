using LabCMS.FixtureDomain.Server.Policies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionPolicyAttribute:Attribute
    {
        public PermissionPolicyAttribute(params Type[] policyTypes)
        {
            if(policyTypes.Any(type => !typeof(IPermissionPolicy).IsAssignableFrom(type)))
            { throw new ArgumentException("policyTypes contains type that doesn't implement IPermissionPolicy",nameof(policyTypes)); }
            PolicyTypes = policyTypes;
        }

        public Type[] PolicyTypes { get; }
    }
}
