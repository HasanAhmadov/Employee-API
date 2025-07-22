using MicroServices.Models;

namespace MicroServices.BusinessLayer.DTOs
{
    public class AttendanceDTO
    {
        public EmployeeDTO Employee { get; set; }
        public DateTime EarliestEnterTime { get; set; }
        public DateTime LatestExitTime { get; set; }
        public Shift EmployeeShift { get; set; }
        public int MinutesLate { get; set; }
    }
}