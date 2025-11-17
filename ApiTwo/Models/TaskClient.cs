using System.Text.Json.Serialization;

namespace ApiTwo.Models
{
    public class TaskClient
    {
        public int ClientId { get; set; }
        [JsonIgnore] public TaskInput Task { get; set; }
        public int TaskId { get; set; }
    }
}
