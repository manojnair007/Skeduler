using Autofac;
using Autofac.Core;
using SmartAppointment.Core.Interfaces;
using SmartAppointment.Core.Interfaces.Repositories;
using SmartAppointment.Core.Provider;
using SmartAppointment.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Services
{
    public abstract class BaseAppContainerService
    {
        private IContainer _container;
        protected ContainerBuilder Builder;
        public BaseAppContainerService()
        {
            Builder = new ContainerBuilder();
            RegisterServices();
            RegisterRepositories();
            RegisterViewModels();
            RegisterModels();
             _container = Builder.Build();
        }

        protected virtual void RegisterServices()
        {
            Builder.RegisterType<CosmosDBProvider>().As<ICosmosDBProvider>().SingleInstance();
            Builder.RegisterType<SettingsService>().As<ISettingsService>().SingleInstance();
            Builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().SingleInstance();
            Builder.RegisterType<HttpService>().As<IHttpService>().SingleInstance();
            Builder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            Builder.RegisterType<CacheService>().As<ICacheService>().SingleInstance();
            Builder.RegisterType<GeoLocationService>().As<IGeoLocationService>().SingleInstance();
            

        }
        protected virtual void RegisterRepositories()
        {
            Builder.RegisterType<TenantsRepository>().As<ITenantsRepository>();
            Builder.RegisterType<CategoriesRepository>().As<ICategoriesRepository>().SingleInstance();
            Builder.RegisterType<UserRepository>().As<IUserRepository>().SingleInstance();
            Builder.RegisterType<AppointmentsRepository>().As<IAppointmentRepository>().SingleInstance();
            
        }
        protected abstract void RegisterViewModels();
        protected abstract void RegisterModels();
        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public object Resolve(Type typeName)
        {
            return _container.Resolve(typeName);
        }

        public T Resolve<T>(params Parameter[] parameters)
        {
            return  _container.Resolve<T>(parameters);
        }
    }
}
