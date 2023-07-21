using RMS.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace RMS.Models
{
    public class RequestViewModel: Request
    {
        public List<Category>? Categories { get; set; }
        public List<Request>? Requests { get; set; }
    }
}
