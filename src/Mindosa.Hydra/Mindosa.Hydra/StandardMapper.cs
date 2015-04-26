// Mindosa.Hydra is largely based on PetaPoco and is bound by the same licensing terms.
// PetaPoco - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

using System;
using System.Reflection;

namespace Mindosa.Hydra
{
    /// <summary>
    /// StandardMapper is the default implementation of IMapper used by PetaPoco
    /// </summary>
    public class StandardMapper : IMapper
    {
        /// <summary>
        /// Constructs a TableInfo for a POCO by reading its attribute data
        /// </summary>
        /// <param name="pocoType">The POCO Type</param>
        /// <returns></returns>
        public TableInfo GetTableInfo(Type pocoType)
        {
            return TableInfo.FromPoco(pocoType);
        }

        /// <summary>
        /// Constructs a ColumnInfo for a POCO property by reading its attribute data
        /// </summary>
        /// <param name="pocoProperty"></param>
        /// <returns></returns>
        public ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
        {
            return ColumnInfo.FromProperty(pocoProperty);
        }

        public Func<object, object> GetFromDbConverter(PropertyInfo TargetProperty, Type SourceType)
        {
            var col = ColumnInfo.FromProperty(TargetProperty);
            return col.CustomMapper;
        }

        public Func<object, object> GetToDbConverter(PropertyInfo SourceProperty)
        {
            return null;
        }
    }
}
