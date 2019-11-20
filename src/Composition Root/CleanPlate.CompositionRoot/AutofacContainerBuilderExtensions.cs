using Autofac;

namespace CleanPlate.CompositionRoot
{
    public static class AutofacContainerBuilderExtensions
    {
        public static ContainerBuilder RegisterModules(this ContainerBuilder builder)
        {
            return builder;
        }
    }
}
