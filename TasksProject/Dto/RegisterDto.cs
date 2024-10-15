using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TasksProject.Models;

namespace TasksProject.Dto
{
    public class RegisterDto 
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
    }
}
