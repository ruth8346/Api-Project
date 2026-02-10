namespace TrickyTrayAPI.DTOs
{
    public class RaffleReportDto
    {
        public string GiftName { get; set; } = "";
        public string WinnerName { get; set; } = "";
        public string WinnerEmail { get; set; } = "";
        public DateTime RaffledAt { get; set; }
    }


}
