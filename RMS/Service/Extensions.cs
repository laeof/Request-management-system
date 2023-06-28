using Microsoft.CodeAnalysis.CSharp.Syntax;
using RMS.Domain;
using RMS.Models;

namespace RMS.Service
{
	public static class Extensions
	{
		public static string CutController(this string str) 
		{
			return str.Replace("Controller", "");
		}
		public static bool ValidateUser(string login, string password, AppDbContext _db, ref uint userId)
		{
			bool isValid = false;

			try
			{
				UserModel user = _db.Users.FirstOrDefault(u => u.Login.ToLower() == login.ToLower() && u.Password == password);

				if (user != null)
				{
					userId = user.Id;
					isValid = true;
				}
			}
			catch
			{
				isValid = false;
			}

			return isValid;
		}
	}
}
