
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TasksProject.DataEntity;
using TasksProject.Dto;
using TasksProject.Models;

namespace TasksProject.Service
{
    public class UserService : IUserService
    {
        private readonly TasksDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordHasher<Users> _passwordHasher = new PasswordHasher<Users>();
        private readonly IMapper _mapper;

        public UserService(TasksDbContext context, IConfiguration configuration, IMapper mapper )
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<UserResponse> LoginAsync(LoginDto loginRequest)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Username == loginRequest.Username);

            if (user != null && VerifyPassword(loginRequest.PasswordHash, user.PasswordHash))
            {
                var token = GenerateJwtToken(user);
                var userResponse = _mapper.Map<UserResponse>(user);
                userResponse.Token = token; 
                return userResponse;
            }
            return null;
        }

        public async Task<bool> RegisterAsync(RegisterDto registerRequest)
        {
            var existingUser = await _context.Users
                            .SingleOrDefaultAsync(u => u.Username == registerRequest.Username || u.Email == registerRequest.Email);

            if (existingUser != null)
            {
                return false; 
            }

            var user = _mapper.Map<Users>(registerRequest);
            user.PasswordHash = HashPassword(registerRequest.PasswordHash);
            user.Role = await _context.Roles.SingleOrDefaultAsync(r => r.Name == "Employee"); ;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }


        private string GenerateJwtToken(Users user)
        {


            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username), 
                new Claim(JwtRegisteredClaimNames.Email, user.Email), 
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "Employee"), 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30), 
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        private string HashPassword(string password)
        {
            return _passwordHasher.HashPassword(new Users(), password);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            Console.WriteLine($"Stored Hash: {storedHash}");
            var result = _passwordHasher.VerifyHashedPassword(null, storedHash, password);
            return result == PasswordVerificationResult.Success;
        }


    }
}
