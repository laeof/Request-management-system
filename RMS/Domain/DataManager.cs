using RMS.Domain.Repositories.Abstract;

namespace RMS.Domain
{
	public class DataManager
	{
		public IRequestRepository Requests { get; set; }
		public ICategoryRepository Categories { get; set; }
		public ILifecycleRepository Lifecycles { get; set; }
		public IUserRepository Users { get; set; }
		public IUserRoleRepository UserRole { get; set; }
        public IRoleRepository Role { get; set; }

        public DataManager(IRequestRepository Requests, 
						   ICategoryRepository Categories,
						   ILifecycleRepository Lifecycle,
						   IUserRepository User,
						   IUserRoleRepository UserRole,
						   IRoleRepository Role)
		{
			this.Requests = Requests;
			this.Categories = Categories;
			this.Lifecycles = Lifecycle;
			this.Users = User;
			this.UserRole = UserRole;
			this.Role = Role;
		}
	}
}
