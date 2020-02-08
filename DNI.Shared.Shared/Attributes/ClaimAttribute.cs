﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class ClaimAttribute : Attribute
    {
        public ClaimAttribute(string claimType = null)
        {
            ClaimType = claimType;
        }

        public string ClaimType { get; }
    }
}