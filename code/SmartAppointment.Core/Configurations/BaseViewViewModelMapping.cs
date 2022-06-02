using System;
using System.Collections.Generic;
using System.Text;

namespace SmartAppointment.Core.Configurations
{
    public abstract class BaseViewViewModelMapping
    {
        protected Dictionary<string, string> Mapping;
        public BaseViewViewModelMapping()
        {
            Mapping = new Dictionary<string, string>();
            AddMapping();
        }

        abstract public void AddMapping();

        
    }
}
