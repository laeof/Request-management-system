﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RMS.Domain.Entities;

namespace RMS.Models
{
	public class PersonalPageViewModel
	{
		public User? User { get; set; }
    }
}
