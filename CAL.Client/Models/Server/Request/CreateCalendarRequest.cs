using CAL.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAL.Client.Models.Server.Request
{
    public class CreateCalendarRequest : IValidatable
    {
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public Guid CalUserId { get; set; }
        public string Color { get; set; } = "";
        private List<string> availableColors = new List<string> { 
            "blue", 
            "red", 
            "green",
            "yellow",
            "purple",
            "orange",
            "pink",
        };

        public bool Validate()
        {
            return availableColors.Contains(Color);
        }
    }
}
