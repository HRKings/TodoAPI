using System.Collections.Generic;
using Core.Utils;

namespace Infrastructure.Interfaces
{
	public interface IUserRepository
	{
		public IList<JwtUtils.User> GetAllUsers();
		public JwtUtils.User GetByEmail(string email);
	}
}