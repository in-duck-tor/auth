namespace InDuckTor.Auth.Infrastructure.Model
{
    public class Credentials
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}
