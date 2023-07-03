using Microsoft.CodeAnalysis.CSharp.Syntax;
using RMS.Domain;
using RMS.Domain.Entities;

namespace RMS.Service
{
    public static class Extensions
	{
		public static string CutController(this string str) 
		{
			return str.Replace("Controller", "");
		}
		public static bool ValidateUser(string login, string password, DataManager dataManager, ref uint userId)
		{

			try
			{
				User user = dataManager.Users.GetUsers().FirstOrDefault(u => u.Login.ToLower() == login.ToLower() && u.Password == password);

				if (user != null)
                {
                    userId = user.Id;
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
