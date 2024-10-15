using Microsoft.AspNetCore.Identity;

namespace TasksProject.Models
{
    public class Roles 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Users> Users { get; set; }
    }
}
