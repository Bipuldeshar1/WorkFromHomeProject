using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Domain.baseModel;
using WorkFromHome.Domain.Enummerations;

namespace WorkFromHome.Domain.models
{
    public class ActiveEmployees:BaseEntity
    {
      
        public Guid EmployeeGUID { get;private  set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public RoleStatusEnums Role { get; private set; }
        public bool IsDeleted { get; private set; }
    }
}
