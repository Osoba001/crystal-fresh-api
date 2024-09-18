namespace Main.Infrastructure.Authentications
{
    public class MainConfigData
    {
        public string? AUTH_SECRET_KEY { get; set; }
        public TimeSpan ACCESS_TOKEN_TTL { get; set; } = TimeSpan.FromMinutes(60);
        public TimeSpan REFRESH_TOKEN_TTL { get; set; } = TimeSpan.FromDays(60);
        public string? MAIN_DB_CONNECT_STRING { get; set; }
    }

}
