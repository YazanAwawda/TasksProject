
using Microsoft.AspNetCore.Identity.Data;
using TasksProject.Dto;

namespace TasksProject.Service
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterDto registerRequest);

        Task<UserResponse> LoginAsync(LoginDto loginRequest);
    }
}
