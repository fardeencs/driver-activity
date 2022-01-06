namespace DriverActivityWeb.Helper
{
    using FluentValidation;
    using FluentValidation.AspNetCore;
    using System.Reflection;



    public class FluentValidationConfig
    {
        //public static void RegisterValidation(ContainerBuilder builder, HttpConfiguration config)
        //{
        //    builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        //        .Where(t => t.Name.EndsWith("Validator"))
        //        .AsImplementedInterfaces()
        //        .InstancePerLifetimeScope();

        //    builder.RegisterType<FluentValidationModelValidatorProvider>().As<ModelValidatorProvider>();

        //    builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        //}

        public static void RegisterValidation(WebApplicationBuilder builder)
        {
            //builder.Services.AddFluentValidation(v => Assembly.GetExecutingAssembly())
            //builder.Services.AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<Assembly>));
            //builder.Services.AddFluentValidation(v => v.RegisterValidatorsFromAssemblyContaining<Authe>));
            //.Where(t => t.GetType().GetEnumName.EndsWith("Validator"))
            //.AsImplementedInterfaces()
            //.InstancePerLifetimeScope();

            //builder.RegisterType<FluentValidationModelValidatorProvider>().As<ModelValidatorProvider>();

            //builder.RegisterType<ValidatorFactory>().As<IValidatorFactory>().SingleInstance();
        }


    }
}
