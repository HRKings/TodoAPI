using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
	public class TodoUpdateDto
	{
		public string Name { get; set; }
		[Required]
		public bool Completed { get; set; }
	}
}