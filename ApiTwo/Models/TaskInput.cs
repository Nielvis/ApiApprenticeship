using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ApiTwo.Models
{
    public class TaskInput
    {

        [Key] public int TaskId { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public bool isComplete { get; set; } = false;
        public DateTime DataCreated { get; set; } = DateTime.UtcNow;

        public ICollection<TaskClient> TaskClients { get; set; }

    }
}
