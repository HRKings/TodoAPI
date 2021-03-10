using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
	public class TodoCreate
	{
		[Required]
		public string Name { get; set; }
		public bool Completed { get; set; }
	}
}