namespace Core.Dtos.JWT
{
    public class AuthDto
    {
        public string Message { get; set; } = "SUCCESS";
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresON { get; set; }
    }
}
