﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabCMS.FixtureDomain.Shared.Events
{
    public abstract record FixtureEvent
    {
        public FixtureEvent(int fixtureNo)
        { FixtureNo = fixtureNo; }
        public int FixtureNo { get; private set; }
        public DateTimeOffset TimeStamp { get; private set; } = DateTimeOffset.Now;
        public abstract string TypeFullName { get; protected set; } 
    }
}
