using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.ApiTrackService
{
    /// <summary>
    /// Configuration for the server.
    /// </summary>
    public class SnakeServiceConfiguration
    {
        private const string SectionRoot = "snake.service";

        private const string PrefixServerConfiguration = "snake";
        private const string KeyServiceName = PrefixServerConfiguration + ".serviceName";
        private const string KeyServiceDisplayName = PrefixServerConfiguration + ".serviceDisplayName";
        private const string KeyServiceDescription = PrefixServerConfiguration + ".serviceDescription";

        private const string DefaultServiceName = "SnakeConsumer";
        private const string DefaultServiceDisplayName = "Snake Consumer Server";
        private const string DefaultServiceDescription = "Snake Consumer Server";

        private static readonly NameValueCollection configuration;

        /// <summary>
        /// Initializes the <see cref="ServiceConfiguration"/> class.
        /// </summary>
        static SnakeServiceConfiguration()
        {
            configuration = (NameValueCollection)ConfigurationManager.GetSection(SectionRoot);
        }

        /// <summary>
        /// Gets the name of the service.
        /// </summary>
        /// <value>The name of the service.</value>
        public static string ServiceName
        {
            get { return GetConfigurationOrDefault(KeyServiceName, DefaultServiceName); }
        }

        /// <summary>
        /// Gets the display name of the service.
        /// </summary>
        /// <value>The display name of the service.</value>
        public static string ServiceDisplayName
        {
            get { return GetConfigurationOrDefault(KeyServiceDisplayName, DefaultServiceDisplayName); }
        }

        /// <summary>
        /// Gets the service description.
        /// </summary>
        /// <value>The service description.</value>
        public static string ServiceDescription
        {
            get { return GetConfigurationOrDefault(KeyServiceDescription, DefaultServiceDescription); }
        }

        /// <summary>
        /// Returns configuration value with given key. If configuration
        /// for the does not exists, return the default value.
        /// </summary>
        /// <param name="configurationKey">Key to read configuration with.</param>
        /// <param name="defaultValue">Default value to return if configuration is not found</param>
        /// <returns>The configuration value.</returns>
        private static string GetConfigurationOrDefault(string configurationKey, string defaultValue)
        {
            string retValue = null;
            if (configuration != null)
            {
                retValue = configuration[configurationKey];
            }

            if (retValue == null || retValue.Trim().Length == 0)
            {
                retValue = defaultValue;
            }
            return retValue;
        }        
    }
}
