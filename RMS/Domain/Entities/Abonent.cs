namespace RMS.Domain.Entities
{
	public class Abonent
	{
		public string? login { get; set; }
		public string? password { get; set; }
		public int? uid { get; set; }
		public string? fio { get; set; }
		public string? phone { get; set; }
		public string? addressFlat { get; set; }
		public string? addressBuild { get; set; }
		public string? addressStreet { get; set; }
	}
}
