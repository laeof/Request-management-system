﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RMS.Models
{
	public class RoleModel
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public uint Id { get; set; }
		public string Name { get; set; }
	}
}
