using Microsoft.Extensions.DependencyInjection;

namespace NetCore_Template.IoC
{
    /// <summary>
    /// Dependancy injection container using .Net Core provider
    /// </summary>
    public class IoCContainer
    {
        public static ServiceProvider ServiceProvider { get; set; }
    }
}
