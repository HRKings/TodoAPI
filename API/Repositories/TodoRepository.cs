using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Models;
using Infrastructure.Database.Context;
using Infrastructure.Interfaces;
using Infrastructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
	public class TodoRepository : ITodoRepository
	{
		private PostgresContext _dbContext;
		
		public TodoRepository(PostgresContext dbContext)
		{
			_dbContext = dbContext;
		}
		
		public async Task<List<Todo>> GetAll() 
			=> await _dbContext.Todos.ToListAsync();
		
		public async Task<PagedData<Todo>> GetPaged(int page, int pageSize)
		{
			var items = await _dbContext.Todos.GetPaged(page, pageSize, teacher => teacher.Id);
			int quantity = await _dbContext.Todos.CountAsync();

			return new PagedData<Todo>()
			{
				TotalItems = quantity,
				PageSize = pageSize,
				Value = items,
			};
		}

		public async ValueTask<Todo> GetFromId(int id) 
			=> await _dbContext.Todos.FindAsync(id);

		public async Task<Todo> Create(TodoCreateDto data)
		{
			var result = await _dbContext.Todos.AddAsync(new Todo
			{
				Name = data.Name,
				Completed = data.Completed
			});

			await _dbContext.SaveChangesAsync();

			return result.Entity;
		}

		public async Task<Todo> Update(int id, TodoUpdateDto data)
		{
			var result = await _dbContext.Todos.FindAsync(id);

			if (result == null)
				return null;

			if (!string.IsNullOrWhiteSpace(data.Name))
				result.Name = data.Name;

			result.Completed = data.Completed;

			_dbContext.Todos.Update(result);
			await _dbContext.SaveChangesAsync();

			return result;
		}

		public Task Delete(int id)
		{
			var result = new Todo {Id = id};
			_dbContext.Entry(result).State = EntityState.Deleted;
			
			return _dbContext.SaveChangesAsync();
		}
	}
}