namespace Daystar.Util.Data
{
    public class WSServerOptions
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string Endpoint { get; set; } = "ws";
    }
}
