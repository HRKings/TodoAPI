using System;
using System.Threading.Tasks;
using API.Repositories;
using Core.Utils;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
	[ApiController]
	[Route("debug/auth")]
	public class AuthController
	{
		public record UserAuth(string Email, string Password);
		
		[HttpPost]
		[AllowAnonymous]
		[Route("login")]
		public async Task<IActionResult> Auth([FromServices] IUserRepository repository, [FromBody] UserAuth user)
		{
			try
			{
				var userExists = new UserRepository().GetByEmail(user.Email);

				if (userExists == null)
					return new BadRequestObjectResult(new {Message = "Email and/or password is invalid."});


				if (userExists.Password != user.Password)
					return new BadRequestObjectResult(new {Message = "Email and/or password is invalid."});
				
				string token = JwtUtils.GenerateToken(userExists);

				return new OkObjectResult(new 
				{
					Token = token,
					User = userExists
				});
			}
			catch (Exception)
			{
				return new BadRequestObjectResult(new { Message = "Internal server error" });
			}
		}
	}
}