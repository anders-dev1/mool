#pragma warning disable 8618
namespace Application.Settings
{
    public interface IJwtSettings
    {
        string Secret { get; }
    }
    
    public class JwtSettings : IJwtSettings
    {
        public string Secret { get; set; }
    }
}