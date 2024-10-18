using Microsoft.AspNetCore.Identity;

namespace TasksProject.Dto
{
    public class LoginDto 
    {
        //public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }

    }
}
