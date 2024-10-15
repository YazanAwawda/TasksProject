using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TasksProject.Models
{
    public class Users 
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public Roles Role { get; set; }
        public ICollection<Tasks> Tasks { get; set; }

    }
}
