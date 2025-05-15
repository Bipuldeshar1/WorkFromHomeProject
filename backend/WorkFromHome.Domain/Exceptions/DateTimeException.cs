using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFromHome.Domain.Exceptions
{
    public class DateTimeException:Exception
    {
        public DateTimeException(string message):base(message)
        {
            
        }
    }
}
