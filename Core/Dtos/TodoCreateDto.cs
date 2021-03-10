using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
	public class TodoCreateDto
	{
		[Required]
		public string Name { get; set; }
		public bool Completed { get; set; }
	}
}