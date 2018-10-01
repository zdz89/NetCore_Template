using Microsoft.Extensions.Configuration;

namespace NetCore_Template.IoC
{
    /// <summary>
    /// Dependancy injection container using .Net Core provider
    /// </summary>
    public class IoC
    {
        public static IConfiguration Configuration { get; set; }
    }
}
