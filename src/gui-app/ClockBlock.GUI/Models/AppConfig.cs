using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClockBlock.GUI.Models
{
    public class AppConfig
    {
        public string WorkingHoursStart { get; set; }
        public string WorkingHoursEnd { get; set; }

        public AppConfig()
        {
            WorkingHoursStart = "09:00";
            WorkingHoursEnd = "17:00";
        }
    }
}
