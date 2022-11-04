#pragma warning disable 8618
namespace Application.Settings
{
    public interface IEnvironmentSettings
    {
        string Url { get; }
    }

    public class EnvironmentSettings : IEnvironmentSettings
    {
        public string Url { get; set; }
    }
}