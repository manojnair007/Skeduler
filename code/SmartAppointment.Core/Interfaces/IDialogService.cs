using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Interfaces
{
    public interface IDialogService
    {
        void ShowLoadingDialog(string message);
        void HideLoadingDialog();
        void ShowDialog(string message);
    }
}
