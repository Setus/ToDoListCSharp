using System;
using System.Configuration;

namespace WebAPI
{
    public class AppConfigUtil
    {
        public AppConfigUtil()
        {
        }

        public static string ReadDatabaseSetting(string key, Boolean throwErrorIfNotFound)
        {
            string property = null;
            try
            {
                var appSettings = ConfigurationManager.AppSettings;

                foreach (var thing in appSettings)
                {
                    Console.WriteLine(thing);
                }
                Console.WriteLine(appSettings.Count);

                property = appSettings[key];

                if (String.IsNullOrEmpty(property) && throwErrorIfNotFound)
                {
                    throw new ArgumentException("Missing value in app.config for key " + key);
                }
                Console.WriteLine("The property is: " + property);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error reading app settings");
            }
            return property;
        }
    }
}
