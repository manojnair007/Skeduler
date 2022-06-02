using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces
{
    public interface INavigationService
    {
        Task NavigateTo(string viewName, string data = null, bool isAnimated = true);
        Task PushAsync(string viewName, string data = null, bool isAnimated = true);
        Task PopAysnc(bool isAnimated = true);
    }
}
