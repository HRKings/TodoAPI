using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Models;
using Dapper;
using Infrastructure.Interfaces;
using Npgsql;

namespace API.Repositories
{
	public class TodoDapperRepository : ITodoRepository
	{
		public async Task<List<Todo>> GetAll()
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();
			
			var result = await connection.QueryAsync<Todo>("SELECT * FROM todos ORDER BY id");
			
			await connection.CloseAsync();
			
			return (List<Todo>) result;
		}

		public async Task<PagedData<Todo>> GetPaged(int page, int pageSize)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();
			
			int skip = (page - 1) * pageSize;
			var items =
				await connection.QueryAsync<Todo>("SELECT * FROM todos ORDER BY id OFFSET @skip LIMIT @pageSize", 
					new {skip, pageSize});
			
			var total = await connection.QueryFirstAsync<int>("SELECT COUNT(*) FROM todos");
			
			await connection.CloseAsync();

			var result = new PagedData<Todo>
			{
				TotalItems = total,
				PageSize = pageSize,
				Value = (List<Todo>) items,
			};

			return result;
		}

		public async ValueTask<Todo> GetFromId(int id)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			await connection.OpenAsync();
			
			var result =
				await connection.QueryFirstAsync<Todo>("SELECT * FROM todos WHERE id = @ID", new {ID = id});
			
			await connection.CloseAsync();
			
			return result;
		}

		public async Task<Todo> Create(TodoCreateDto data)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			
			var resultingId =
				await connection.ExecuteScalarAsync<int>("INSERT INTO todos(name, completed) VALUES (@name, @completed) RETURNING id", 
					new
					{
						name = data.Name,
						completed = data.Completed
					});
			
			var result = await connection.QueryFirstAsync<Todo>("SELECT * FROM todos WHERE id = @ID", new {ID = resultingId});
			
			await connection.CloseAsync();
			return result;
		}

		public async Task<Todo> Update(int id, TodoUpdateDto data)
		{
			await using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));
			
			await connection.OpenAsync();

			int resultingId;
			if(!string.IsNullOrWhiteSpace(data.Name))
				resultingId = await connection.QueryFirstAsync<int>("UPDATE todos SET name = @name WHERE id = @ID RETURNING id",
					new {name = data.Name, ID = id});
			
			resultingId = await connection.QueryFirstAsync<int>("UPDATE todos SET completed = @completed WHERE id = @ID RETURNING id",
				new {completed = data.Completed, ID = id});
			
			var result = await connection.QueryFirstAsync<Todo>("SELECT * FROM todos WHERE id = @ID", new {ID = resultingId});
			
			await connection.CloseAsync();
			return result;
		}

		public async Task Delete(int id)
		{
			using var connection =
				new NpgsqlConnection(Environment.GetEnvironmentVariable("DATABASE"));

			await connection.OpenAsync();
			
			var result = await connection.ExecuteAsync("DELETE FROM todos WHERE id = @ID", new {ID = id});
			
			await connection.CloseAsync();
		}
	}
}