﻿
namespace TasksProject.Dto
{
    public class UserDto 
    {
        public int ? Id { get; set; }
        public string ? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public int RoleId { get; set; }
    }
}
