using Microsoft.JSInterop.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace ApiOne.Models
{
    public class ClientInput
    {
        [Key] public int ClientId { get; set; }

        public string Name { get; set; } 

        public string Email { get; set; } 
    }
}
