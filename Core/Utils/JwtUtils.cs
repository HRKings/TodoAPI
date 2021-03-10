using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Enums;
using Microsoft.IdentityModel.Tokens;

namespace Core.Utils
{
	public static class JwtUtils
	{
		public record User(EnumUserType Type, string Email, string Name, string Password);

		public static string GenerateToken(User user)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			
			var key = Encoding.ASCII.GetBytes(
				Environment.GetEnvironmentVariable("JWT_SECRET") 
				?? "YOU_SHOULD_HAVE_A_SECRET");
            
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new(ClaimTypes.Name, user.Name),
					new(ClaimTypes.Role, user.Type.ToString())
				}),
				Expires = DateTime.UtcNow.AddHours(10),

				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
            
			var token = tokenHandler.CreateToken(tokenDescriptor);
            
			return tokenHandler.WriteToken(token);
		}
	}
}