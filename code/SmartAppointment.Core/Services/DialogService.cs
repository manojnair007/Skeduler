using Acr.UserDialogs;
using SmartAppointment.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Services
{
    public class DialogService : IDialogService
    {
        public void HideLoadingDialog()
        {
            UserDialogs.Instance.HideLoading();
        }

        public void ShowLoadingDialog(string message)
        {
            UserDialogs.Instance.ShowLoading(message);
        }

        public void ShowDialog(string message)
        {
            UserDialogs.Instance.Alert(message);
        }
    }
}
