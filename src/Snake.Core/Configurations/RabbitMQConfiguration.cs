using Snake.Core.Util;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake.Core.Configurations
{
    public class RabbitMQConfiguration
    {
        private const string SectionRoot = "RabbitMQ.Connection";

        private const string PrefixServerConfiguration = "RabbitMQ";
        private const string KeyHostName = PrefixServerConfiguration + ".HostName";
        private const string KeyPort = PrefixServerConfiguration + ".Port";
        private const string KeyQueueName = PrefixServerConfiguration + ".QueueName";
        private const string KeyUserName = PrefixServerConfiguration + ".UserName";
        private const string KeyPassword = PrefixServerConfiguration + ".Password";
        private const string KeyVirtualHost = PrefixServerConfiguration + ".VirtualHost";
        private const string KeyConsumerNum = PrefixServerConfiguration + ".ConsumerNum";
        private const string KeyUseRetryNum = PrefixServerConfiguration + ".UseRetryNum";
        private const string KeyUseRateLimit = PrefixServerConfiguration + ".UseRateLimit";

        private const string DefaultHostName = "";
        private const string DefaultUserName = "";
        private const string DefaultPassword = "";
        private const string DefaultVirtualHost = "Dev";
        private const string DefaultPort = "5672";
        private const string DefaultQueueName = "testqueue";
        private const string DefaultConsumerNum = "1";
        private const string DefaultUseRetryNum = "3";
        private const string DefaultUseRateLimit = "1000";

        private static readonly NameValueCollection configuration;

        /// <summary>
        /// Initializes the <see cref="Configuration"/> class.
        /// </summary>
        static RabbitMQConfiguration()
        {
            configuration = (NameValueCollection)ConfigurationManager.GetSection(SectionRoot);
        }

        /// <summary>
        /// Gets the HostName of RabbitMQ.
        /// </summary>
        /// <value>The name of the service.</value>
        public static string HostName
        {
            get { return GetConfigurationOrDefault(KeyHostName, DefaultHostName); }
        }

        public static string Port
        {
            get { return GetConfigurationOrDefault(KeyPort, DefaultPort); }
        }

        public static string QueueName
        {
            get { return GetConfigurationOrDefault(KeyQueueName, DefaultQueueName); }
        }
        /// <summary>
        /// Gets the UserName of RabbitMQ.
        /// </summary>
        /// <value>The display name of the service.</value>
        public static string UserName
        {
            get { return GetConfigurationOrDefault(KeyUserName, DefaultUserName); }
        }

        /// <summary>
        /// Gets the Password of RabbitMQ.
        /// </summary>
        /// <value>The service description.</value>
        public static string Password
        {
            get { return GetConfigurationOrDefault(KeyPassword, DefaultPassword); }
        }

        public static string VirtualHost
        {
            get { return GetConfigurationOrDefault(KeyVirtualHost, DefaultVirtualHost); }
        }

        /// <summary>
        /// 消费者个数
        /// </summary>
        public static int ConsumerNum
        {
            get
            {
                int consumerNum = StringHelper.Toint(GetConfigurationOrDefault(KeyConsumerNum, DefaultConsumerNum));
                return consumerNum == -1 ? 1 : consumerNum;
            }
        }

        /// <summary>
        /// 重试次数
        /// </summary>
        public static int UseRetryNum
        {
            get
            {
                int useRetryNum = StringHelper.Toint(GetConfigurationOrDefault(KeyUseRetryNum, DefaultUseRetryNum));
                return useRetryNum == -1 ? 3 : useRetryNum;
            }
        }

        /// <summary>
        /// 每分钟限数器
        /// </summary>
        public static int UseRateLimit
        {
            get
            {
                int useRateLimit = StringHelper.Toint(GetConfigurationOrDefault(KeyUseRateLimit, DefaultUseRateLimit));
                return useRateLimit == -1 ? 3 : useRateLimit;
            }
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
