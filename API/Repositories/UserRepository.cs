using System;
using System.Collections.Generic;
using System.Linq;
using Core.Enums;
using Core.Utils;
using Infrastructure.Interfaces;

namespace API.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IList<JwtUtils.User> _users = new List<JwtUtils.User>
		{

			new
			(
				EnumUserType.Admin,
				"admin@test.com",
				"Hector",
				"123"
			),

			new
			(
				EnumUserType.Moderator,
				"mod@test.com",
				"Maria",
				"123"
			),

			new
			(
				EnumUserType.User,
				"user@test.com",
				"Jonas",
				"123"
			),
		};

		public IList<JwtUtils.User> GetAllUsers() => _users;

		public JwtUtils.User GetByEmail(string email)
			=> _users.FirstOrDefault(user 
				=> string.Equals(user.Email, email, StringComparison.CurrentCultureIgnoreCase));
	}
}