﻿// Mindosa.Hydra is largely based on PetaPoco and is bound by the same licensing terms.
// PetaPoco - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

using System;

namespace Mindosa.Hydra.Attributes
{
    /// <summary>
    /// Use the Ignore attribute on POCO class properties that shouldn't be mapped
    /// by AsyncPoco.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute
    {
    }
}
