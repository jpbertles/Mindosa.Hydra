// Mindosa.Hydra is largely based on PetaPoco and is bound by the same licensing terms.
// PetaPoco - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mindosa.Hydra.Internal
{
    internal static class Mappers
    {
        /// <summary>
        /// Registers a mapper for all types in a specific assembly
        /// </summary>
        /// <param name="assembly">The assembly whose types are to be managed by this mapper</param>
        /// <param name="mapper">The IMapper implementation</param>
        public static void Register(Assembly assembly, IMapper mapper)
        {
            RegisterInternal(assembly, mapper);
        }

        /// <summary>
        /// Registers a mapper for a single POCO type
        /// </summary>
        /// <param name="type">The type to be managed by this mapper</param>
        /// <param name="mapper">The IMapper implementation</param>
        public static void Register(Type type, IMapper mapper)
        {
            RegisterInternal(type, mapper);
        }

        /// <summary>
        /// Remove all mappers for all types in a specific assembly
        /// </summary>
        /// <param name="assembly">The assembly whose mappers are to be revoked</param>
        public static void Revoke(Assembly assembly)
        {
            RevokeInternal(assembly);
        }

        /// <summary>
        /// Remove the mapper for a specific type
        /// </summary>
        /// <param name="type">The type whose mapper is to be removed</param>
        public static void Revoke(Type type)
        {
            RevokeInternal(type);
        }

        /// <summary>
        /// Revoke an instance of a mapper
        /// </summary>
        /// <param name="mapper">The IMapper to be revkoed</param>
        public static void Revoke(IMapper mapper)
        {
            _lock.EnterWriteLock();
            try
            {
                foreach (var i in _mappers.Where(kvp => kvp.Value == mapper).ToList())
                    _mappers.Remove(i.Key);
            }
            finally
            {
                _lock.ExitWriteLock();
                FlushCaches();
            }
        }

        /// <summary>
        /// Retrieve the IMapper implementation to be used for a specified POCO type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static IMapper GetMapper(Type t)
        {
            _lock.EnterReadLock();
            try
            {
                IMapper val;
                if (_mappers.TryGetValue(t, out val))
                    return val;
                if (_mappers.TryGetValue(t.Assembly, out val))
                    return val;

                return Singleton<StandardMapper>.Instance;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }


        static void RegisterInternal(object typeOrAssembly, IMapper mapper)
        {
            _lock.EnterWriteLock();
            try
            {
                _mappers.Add(typeOrAssembly, mapper);
            }
            finally
            {
                _lock.ExitWriteLock();
                FlushCaches();
            }
        }

        static void RevokeInternal(object typeOrAssembly)
        {
            _lock.EnterWriteLock();
            try
            {
                _mappers.Remove(typeOrAssembly);
            }
            finally
            {
                _lock.ExitWriteLock();
                FlushCaches();
            }
        }

        static void FlushCaches()
        {
            // Whenever a mapper is registered or revoked, we have to assume any generated code is no longer valid.
            // Since this should be a rare occurance, the simplest approach is to simply dump everything and start over.

            //TODO: figure out what we want to do with the MultiPocoFactory
            //MultiPocoFactory.FlushCaches();
            PocoData.FlushCaches();
        }

        static Dictionary<object, IMapper> _mappers = new Dictionary<object, IMapper>();
        static ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
    }
}
