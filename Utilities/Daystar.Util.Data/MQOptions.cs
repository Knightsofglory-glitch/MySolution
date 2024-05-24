// The following section needs to be added to appsettings.json
// "ActivityMQOptions": {
//   "UseMQ": true,
//   "UserName": "guest",
//   "Password": "guest",
//   "HostName": "localhost",
//   "VHost": "/",
//   "Port": 5672,
//   "ExchangeName": "Daystar.Flow.Exchange.Activity",
//   "QueueRoutes": "Daystar.Flow"
// },

namespace Daystar.Util.Data
{
    public class MQOptions
    {
        public bool UseMQ { get; set; } = false;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string HostName { get; set; }
        public string VHost { get; set; } = "/";
        public int Port { get; set; } = 5672;
        public string ExchangeName { get; set; }
        public string QueueRoutes { get; set; }
    }
}
