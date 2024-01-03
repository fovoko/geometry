using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace gems
{
    /// <summary>
    /// The entry point of the class 
    /// </summary>
    sealed public class Program
    {
        /// <summary>
        /// Entry point of the app
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Configure application
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                 {
                     webBuilder.UseStartup<Startup>();
                 });
        }
    }
}
