#nullable disable

namespace Core.Models
{
    public partial class Todo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
    }
}
