﻿// Mindosa.Hydra is largely based on PetaPoco and is bound by the same licensing terms.
// PetaPoco - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

using System;

namespace Mindosa.Hydra.Attributes
{
    /// <summary>
    /// Specifies the primary key column of a poco class, whether the column is auto incrementing
    /// and the sequence name for Oracle sequence columns.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(string primaryKey)
        {
            Value = primaryKey;
            autoIncrement = !primaryKey.Contains(",");
        }

        public string Value
        {
            get;
            private set;
        }

        public string sequenceName
        {
            get;
            set;
        }

        public bool autoIncrement
        {
            get;
            set;
        }
    }
}
