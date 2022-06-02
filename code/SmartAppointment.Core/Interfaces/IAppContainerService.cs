using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Interfaces
{
    public interface IAppContainerService
    {
        T Resolve<T>();
        object Resolve(Type typeName);
        T Resolve<T>(params Parameter[] parameters);
    }
}
