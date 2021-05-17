namespace JwtBearerAuthentication.Configuration
{
    public class JwtConfiguration
    {
        public string ValidIssuer { get; set; }

        public string ValidAudience { get; set; }

        public string Secret { get; set; }
    }
}
