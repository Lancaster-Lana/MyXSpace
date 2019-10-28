using Autofac;
using System.Reflection;

namespace MyXSpace.Web
{
    public class WebModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            //MyXSpace.Web.Assembly
            var assemblyType = Assembly.GetExecutingAssembly().GetType();

            builder.RegisterAssemblyTypes(assemblyType.GetTypeInfo().Assembly).AsImplementedInterfaces();
        }
    }
}

