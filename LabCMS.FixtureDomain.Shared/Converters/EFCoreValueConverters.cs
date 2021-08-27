using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using LabCMS.FixtureDomain.Shared.Events;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace LabCMS.FixtureDomain.Shared.Converters
{
    public static class EFCoreValueConverters
    {
        private static readonly Assembly _assembly = typeof(FixtureEvent).Assembly;
       
    }
}
