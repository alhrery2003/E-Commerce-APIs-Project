using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductSystem.BLL.DTOs;
using ProductSystem.BLL.Shared;
using ProductSystem.DAL.Entities;

namespace ProductSystem.BLL.Interfaces
{
    public interface IAuthManager
    {
        Task<Result<AuthResponseDto>> RegisterAsync(RegisterDto dto);
        Task<Result<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}
