using CAL.Client.Interfaces;

namespace CAL.Client.Models.Server.Request
{
    public class CreateCalUserRequest : IValidatable
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool Validate()
        {
            return FirstName != LastName;
        }
    }
}

