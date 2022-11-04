using Shared;

#pragma warning disable 8618
namespace Application.Settings
{
    public class AppSettings
    {
        public EnvironmentSettings Environment { get; set; }
        public MongoDbSettings MongoDb { get; set; }
        public JwtSettings Jwt { get; set; }
    }
}