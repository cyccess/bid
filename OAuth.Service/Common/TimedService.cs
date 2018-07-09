using FluentScheduler;

namespace OAuth.Service.Common
{
    public class TimedService : Registry
    {
        public TimedService() {
            System.Console.WriteLine("ABC");
        }
    }
}
