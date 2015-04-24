﻿// Mindosa.Hydra is largely based on PetaPoco and is bound by the same licensing terms.
// PetaPoco - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

using System;

namespace Mindosa.Hydra.Attributes
{
    /// <summary>
    /// Marks a poco property as a computed column that is populated in queries
    /// but not used for updates or inserts.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedColumnAttribute : ColumnAttribute
    {
        public ComputedColumnAttribute()
        {
        }

        public ComputedColumnAttribute(string name)
            : base(name)
        {
        }
    }
}
