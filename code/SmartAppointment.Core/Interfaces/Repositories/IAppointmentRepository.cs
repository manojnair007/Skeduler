using Appointment.Core.Entities;
using Appointment.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartAppointment.Core.Interfaces.Repositories
{
    public interface IAppointmentRepository
    {
        Task<AppointmentDetails> InsertItemAsync(AppointmentDetails appointmentDetails);
        Task<List<AppointmentDetails>> GetAppointmentsAysnc(string date, string timeSlot, string tenantId);
        Task<List<AppointmentDetails>> GetAppointmentsAysnc(string userId);
        Task<AppointmentDetails> UpsertDocumentAsync(AppointmentDetails appointmentDetails);
    }
}
