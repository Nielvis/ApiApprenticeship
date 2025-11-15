using System.Data;

namespace ApiTwo.Models
{
    public class TaskInput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public bool isComplete { get; set; } = false;
        public DateTime DataCreated { get; set; } = DateTime.UtcNow;

    }
}
