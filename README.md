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
