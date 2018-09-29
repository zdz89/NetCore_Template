using Microsoft.Extensions.DependencyInjection;
using NetCore_Template.Data;

namespace NetCore_Template.IoC
{
    /// <summary>
    /// Stores DI services
    /// </summary>
    public class IoC
    {
        public static ApplicationDbContext ApplicationDbContext => IoCContainer.ServiceProvider.GetService<ApplicationDbContext>();
    }
}
