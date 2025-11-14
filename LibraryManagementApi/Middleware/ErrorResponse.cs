namespace LibraryManagementApi.Middleware
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string ErrorCode { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public Dictionary<string, string[]>? ValidationErrors { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Path { get; set; } = string.Empty;
    }
}
