using RMS.Domain.Entities;

namespace RMS.Models
{
    public class BrigadeViewModel: Brigade
    {
        public List<Brigade>? ViewBrigades { get; set; }
        public List<User>? Mounters { get; set; }
    }
}
