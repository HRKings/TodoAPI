using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dtos;
using Core.Models;

namespace Infrastructure.Interfaces
{
	public interface ITodoRepository
	{
		public Task<List<Todo>> GetAll();

		public Task<PagedData<Todo>> GetPaged(int page, int pageSize);

		public ValueTask<Todo> GetFromId(int id);

		public Task<Todo> Create(TodoCreateDto data);

		public Task<Todo> Update(int id, TodoUpdateDto data);

		public Task Delete(int id);
	}
}