using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFromHome.Domain.Exceptions
{
    public class EmployeeNotFoundException:Exception
    {
        public EmployeeNotFoundException(string message) : base(message)
        {

        }
    }
}

