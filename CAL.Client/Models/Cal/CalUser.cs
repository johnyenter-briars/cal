using CAL.Client.Models.Cal.Request;

namespace CAL.Client.Models.Cal
{
    public class CalUser
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CreateCalUserRequest ToRequest()
        {
            return new CreateCalUserRequest
            {
                FirstName = FirstName,
                LastName = LastName,
            };
        }
    }
}
