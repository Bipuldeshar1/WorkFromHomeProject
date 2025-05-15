using System.Text.Json.Serialization;
using WorkFromHome.Domain.Enummerations;

namespace WFH.Api.dto
{
    public class EmployeeDto
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public string Email { get;  set; }
        public string PhoneNumber { get;  set; }
        public string Role { get;  set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
   
}
