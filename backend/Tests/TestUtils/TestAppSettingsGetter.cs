using System.IO;
using System.Reflection;
using API;
using Application.Settings;
using Microsoft.Extensions.Configuration;

namespace Tests.TestUtils
{
    public static class TestAppSettingsGetter
    {
        public static AppSettings Get()
        {
            var apiAssembly = Assembly.GetAssembly(typeof(Startup));
            var apiLocation = Path.GetDirectoryName(apiAssembly!.Location);
            
            var cb = new ConfigurationBuilder()
                .SetBasePath(apiLocation)
                .AddJsonFile("appsettings.json");
            cb.AddJsonFile($"appsettings.Development.json");
            
            var configuration = cb.Build();
            var settings = configuration.Get<AppSettings>();
            return settings;
        }
    }
}