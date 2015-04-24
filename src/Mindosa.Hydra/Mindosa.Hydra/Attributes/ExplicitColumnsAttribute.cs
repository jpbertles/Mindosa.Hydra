﻿// Mindosa.Hydra is largely based on PetaPoco and is bound by the same licensing terms.
// PetaPoco - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

using System;

namespace Mindosa.Hydra.Attributes
{
    /// <summary>
    /// Poco classes marked with the Explicit attribute require all column properties to 
    /// be marked with the Column attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ExplicitColumnsAttribute : Attribute
    {
    }
}
