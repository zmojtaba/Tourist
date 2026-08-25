namespace Backend.Infrustructure.Models
{
    public class ApplicationUserLogInHistory
    {
        public string Id { get; set; } = default!;
        public long LoginTime { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
