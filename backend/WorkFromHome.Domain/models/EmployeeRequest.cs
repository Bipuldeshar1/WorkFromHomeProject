
using System;
using WorkFromHome.Domain.Enummerations;

namespace WorkFromHome.Domain.models
{
    public class EmployeeRequest
    {
        public Guid EmployeeGuid { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }    
        public string Email {  get; private set; }  
        public RoleStatusEnums Role { get; private set; }
        public Guid RequestGuid { get; private set; }
        public DateTime RequestFrom { get; private set; }
        public DateTime RequestUpTo { get; private set; }
        public RequestEnums RequestStatus { get; private set; }
    }
}
