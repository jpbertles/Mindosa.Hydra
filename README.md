# Mindosa.Hydra
Inspired by and based on the micro-ORMs PetaPoco and NPoco. The goal of this library is to give a simple, extension based API on top of ADO.Net. Specifically, making the hydration of multiple result sets simple and straight forward, particularly when doing so asynchronously.

```csharp
public class User 
{
    public int UserId { get;set; }
    public string Email { get;set; }
}

using (var connection = new SqlConnection(connectionString))
{
	using (var cmd = connection.CreateCommand(sql))
	{
		cmd.Connection.Open();
		using (var reader = cmd.ExecuteReader())
		{
			var firstResultSet = reader.Hydrate<User>();
			var secondResultSet = reader.Hydrate<User>();
		}
	}
}


using (var connection = new SqlConnection(connectionString))
{
	using (var cmd = connection.CreateCommand(sql))
	{
		cmd.Connection.Open();
		using (var reader = await cmd.ExecuteReaderAsync())
		{
			var firstResultSet = await reader.HydrateAsync<User>();
			var secondResultSet = await reader.HydrateAsync<User>();
		}
	}
}
```

For more complicated scenarios you can implement custom mapping that will be applied during hydration. For example, if you are returning xml in your result set, you can use the CustomMapping attribute to map the resultset appropriately.

```csharp
public class Location
{
	public string Address { get; set; }
	public string PostalCode { get; set; }
}

public class User 
{
    public int UserId { get;set; }
    public string Email { get;set; }
	[CustomMapping("CustomData", typeof(XmlMapper<Location>))]
	public Location Location { get; set; }
}

public class XmlMapper<T> : ICustomMapper
{
	public Func<object, object> GetMapper()
	{
		return x =>
		{
			using (var reader = new StringReader(x.ToString()))
				return (T)(new XmlSerializer(typeof(T)).Deserialize(reader));
		};
	}
}
```