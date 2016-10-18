using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using KafkaNet;
using KafkaNet.Model;
using KafkaNet.Protocol;
using log4net;
using log4net.Config;

[assembly: XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

namespace CommonLib
{
    public class KafkaService
    {
        private static string kafkaHost = ConfigurationManager.ConnectionStrings["Kafka"].ConnectionString;
        private static KafkaOptions options;
        private static BrokerRouter router;
        private static Producer client;

        public KafkaService()
        {
            if (options == null)
            {
                options = new KafkaOptions(new Uri(kafkaHost))
                {
                    Log = new KafkaLog()
                };
            }
            if (router == null)
            {
                router = new BrokerRouter(options);
            }
            if (client == null)
            {
                client = new Producer(router)
                {
                    BatchSize = 100,
                    BatchDelayTime = TimeSpan.FromMilliseconds(2000)
                };
            }
        }

        public void Send(string topicName, string message)
        {
            Task.Run(() =>
            {
                try
                {
                    client.SendMessageAsync(topicName, new[] { new Message(message) }).Wait();
                }
                catch (Exception ex)
                {

                }
            });
        }


        public class KafkaLog : IKafkaLog
        {
            private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            public KafkaLog() { }
            public void DebugFormat(string format, params object[] args)
            {
                //Because there is no global configure, so comment debug
                //log.DebugFormat(format, args);
            }
            public void ErrorFormat(string format, params object[] args)
            {
                log.ErrorFormat(format, args);
            }
            public void FatalFormat(string format, params object[] args)
            {
                log.FatalFormat(format, args);
            }
            public void InfoFormat(string format, params object[] args)
            {
                log.InfoFormat(format, args);
            }
            public void WarnFormat(string format, params object[] args)
            {
                log.WarnFormat(format, args);
            }
        }
    }
}
