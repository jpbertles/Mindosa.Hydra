﻿// Mindosa.Hydra is largely based on PetaPoco and is bound by the same licensing terms.
// PetaPoco - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

using System;
using System.Reflection;
using Mindosa.Hydra.Attributes;

namespace Mindosa.Hydra
{
    /// <summary>
    /// Hold information about a column in the database.
    /// </summary>
    /// <remarks>
    /// Typically ColumnInfo is automatically populated from the attributes on a POCO object and it's properties. It can
    /// however also be returned from the IMapper interface to provide your owning bindings between the DB and your POCOs.
    /// </remarks>
    public class ColumnInfo
    {
        /// <summary>
        /// The SQL name of the column
        /// </summary>
        public string ColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// True if this column returns a calculated value from the database and shouldn't be used in Insert and Update operations.
        /// The column will not be included in Select operations by default.
        /// </summary>
        public bool ResultColumn
        {
            get;
            set;
        }

        /// <summary>
        /// True if this column returns a calculated value from the database and shouldn't be used in Insert and Update operations.
        /// The column will be included in Select operations by default.
        /// </summary>
        public bool ComputedColumn
        {
            get;
            set;
        }

        /// <summary>
        /// True if time and date values returned through this column should be forced to UTC DateTimeKind. (no conversion is applied - the Kind of the DateTime property
        /// is simply set to DateTimeKind.Utc instead of DateTimeKind.Unknown.
        /// </summary>
        public bool ForceToUtc
        {
            get;
            set;
        }

        public Func<object, object> CustomMapper { get; set; } 

        /// <summary>
        /// Creates and populates a ColumnInfo from the attributes of a POCO property.
        /// </summary>
        /// <param name="pi">The property whose column info is required</param>
        /// <returns>A ColumnInfo instance</returns>
        public static ColumnInfo FromProperty(PropertyInfo pi)
        {
            // Check if declaring poco has [Explicit] attribute
            bool ExplicitColumns = pi.DeclaringType.GetCustomAttributes(typeof(ExplicitColumnsAttribute), true).Length > 0;

            // Check for [Column]/[Ignore] Attributes
            var ColAttrs = pi.GetCustomAttributes(typeof(ColumnAttribute), true);
            if (ExplicitColumns)
            {
                if (ColAttrs.Length == 0)
                    return null;
            }
            else
            {
                if (pi.GetCustomAttributes(typeof(IgnoreAttribute), true).Length != 0)
                    return null;
            }

            ColumnInfo ci = new ColumnInfo();

            // Read attribute
            if (ColAttrs.Length > 0)
            {
                var colattr = (ColumnAttribute)ColAttrs[0];

                ci.ColumnName = colattr.Name == null ? pi.Name : colattr.Name;
                ci.ForceToUtc = colattr.ForceToUtc;
                if ((colattr as ResultColumnAttribute) != null)
                    ci.ResultColumn = true;
                if ((colattr as ComputedColumnAttribute) != null)
                    ci.ComputedColumn = true;
                if ((colattr as CustomMappingAttribute) != null)
                {
                    var customMapper = Activator.CreateInstance((colattr as CustomMappingAttribute).Mapper) as ICustomMapper;
                    ci.CustomMapper = customMapper.GetMapper();
                }
            }
            else
            {
                ci.ColumnName = pi.Name;
                ci.ForceToUtc = false;
                ci.ResultColumn = false;
                ci.ComputedColumn = false;
            }

            return ci;
        }
    }
}
