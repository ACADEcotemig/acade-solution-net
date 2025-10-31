using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AcadeAppApi.Models;

namespace AcadeAppApi.Services;

public interface ITokenService
{
 string CreateToken(Usuario user);
}

public class JwtTokenService : ITokenService
{
 private readonly byte[] _key;
 private readonly int _expiryMinutes;

 public JwtTokenService(byte[] key, int expiryMinutes =60)
 {
 _key = key;
 _expiryMinutes = expiryMinutes;
 }

 public string CreateToken(Usuario user)
 {
 var claims = new[]
 {
 new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
 new Claim(JwtRegisteredClaimNames.UniqueName, user.Nome ?? string.Empty),
 new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty)
 };

 var credentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
 var token = new JwtSecurityToken(
 claims: claims,
 expires: DateTime.UtcNow.AddMinutes(_expiryMinutes),
 signingCredentials: credentials
 );

 return new JwtSecurityTokenHandler().WriteToken(token);
 }
}
