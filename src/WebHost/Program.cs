using NetModular.Lib.Host.Web;

namespace NetModular.Module.Quartz.WebHost
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new HostBuilder().RunAsync(args);
        }
    }
}
