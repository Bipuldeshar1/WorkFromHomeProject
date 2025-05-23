﻿using WorkFromHome.Domain.Enummerations;

namespace WFH.Api.DTO
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid EmployeeGuid { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
    }
}
