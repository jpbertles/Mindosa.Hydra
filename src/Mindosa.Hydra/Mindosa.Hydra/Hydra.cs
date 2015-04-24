using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mindosa.Hydra.Internal;

namespace Mindosa.Hydra
{
    public static class Hydra
    {
        public static List<T> Hydrate<T>(this IDataReader reader, string key = null, bool getNextResult = true)
        {
            var results = new List<T>();

            if (!reader.IsClosed)
            {
                var type = typeof (T);
                var pd = PocoData.ForType(type);
                
                var factory = pd.GetFactory(type.FullName, key ?? string.Empty, 0, reader.FieldCount, reader) as Func<IDataReader, T>;

                while (reader.Read())
                {
                    T poco;
                    try
                    {
                        poco = factory(reader);
                    }
                    catch (Exception x)
                    {
                        System.Diagnostics.Debug.WriteLine(x.ToString());
                        throw;
                    }

                    results.Add(poco);
                }

                if (getNextResult)
                {
                    reader.NextResult();
                }
            }

            return results;
        }

        public static async Task<List<T>> HydrateAsync<T>(this DbDataReader reader, string key = null, bool getNextResult = true)
        {
            var results = new List<T>();

            if (!reader.IsClosed)
            {
                var type = typeof(T);
                var pd = PocoData.ForType(type);

                var factory = pd.GetFactory(type.FullName, key ?? string.Empty, 0, reader.FieldCount, reader) as Func<IDataReader, T>;

                while (await reader.ReadAsync())
                {
                    T poco;
                    try
                    {
                        poco = factory(reader);
                    }
                    catch (Exception x)
                    {
                        System.Diagnostics.Debug.WriteLine(x.ToString());
                        throw;
                    }

                    results.Add(poco);
                }

                if (getNextResult)
                {
                    await reader.NextResultAsync();
                }
            }

            return results;
        } 
    }
}
