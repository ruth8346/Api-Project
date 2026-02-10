namespace TrickyTrayAPI.Models
{
    public class SystemState
    {
        public enum SaleStatus
        {
            Draft = 0,
            Active = 1,
            Finished = 2
        }
        public int Id { get; set; } = 1;
        public SaleStatus Status { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }

}
