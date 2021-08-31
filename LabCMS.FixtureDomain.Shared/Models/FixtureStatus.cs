using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Shared.Events;

namespace LabCMS.FixtureDomain.Shared.Models
{
    public enum FixtureStatus
    {
        Unknown = 0,
        Registered,
        AcceptanceChecked,
        InternalCheckoutApply,
        ExternalCheckoutApply,
        ExternalCheckoutApprove,
        CheckedOut,
        FixtureRoom
    }
}
