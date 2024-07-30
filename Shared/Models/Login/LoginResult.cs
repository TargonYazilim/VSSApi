namespace Shared.Models.Login
{
    public class LoginResult
    {
        public string Error { get; set; }
        public string Result { get; set; }
        public string? Token { get; set; }
        public int Id { get; set; }

    }
}
