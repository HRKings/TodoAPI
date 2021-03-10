using System.Collections.Generic;

namespace Core.Models
{
	public struct PagedData<T>
	{
		public int TotalItems { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<T> Value { get; set; }
	}
}