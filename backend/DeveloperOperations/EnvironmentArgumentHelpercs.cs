using System;

namespace DeveloperOperations
{
    public static class EnvironmentArgumentHelper
    {
        public static string GetConfigStringFromArgs(string[] args)
        {
            var environment = ExtractFromArgs(args);
            var configString = GetConfigStringFromEnvironment(environment);
            return configString;
        }
        
        public static Environment ExtractFromArgs(string[] args)
        {
            var environmentIndex = Array.IndexOf(args, "--env");
            if (environmentIndex == -1)
            {
                return Environment.Local;
            }
            
            var environmentString = args[environmentIndex+1];
            switch (environmentString)
            {
                case "local":
                    return Environment.Local;
                case "staging":
                    return Environment.Staging;
                case "production":
                    return Environment.Production;
            }

            throw new Exception(
                $"Environment: {environmentString} is not valid. Use either: local, staging or production");
        }

        public static string GetConfigStringFromEnvironment(Environment environment)
        {
            return environment switch
            {
                Environment.Local => "Development",
                Environment.Staging => "Staging",
                Environment.Production => "Production",
                _ => throw new ArgumentOutOfRangeException(nameof(environment), environment, null)
            };
        }
    }

    public enum Environment
    {
        Local,
        Staging,
        Production
    }
}