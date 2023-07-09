using Microsoft.CodeAnalysis.CSharp.Syntax;
using RMS.Domain;
using RMS.Domain.Entities;
using System.Security.Policy;

namespace RMS.Service
{
    public static class Extensions
	{
		public static string CutController(this string str) 
		{
			return str.Replace("Controller", "");
		}
		public static async Task<bool> ValidateUser(string login, string password, DataManager dataManager)
		{

			try
			{

				User user = dataManager.Users.GetUsers().AsEnumerable().FirstOrDefault(u => u.Login.ToLower() == login.ToLower() && SecurePasswordHasher.Verify(password, u.Password));

				if (user != null)
                {
                    if (!user.IsActive)
						return false;
                    return true;
				}
			}
			catch
			{
				return false;
			}
			return false;
		}
	}
}
