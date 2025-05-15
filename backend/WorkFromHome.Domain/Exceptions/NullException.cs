using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFromHome.Domain.Exceptions
{
    public class NullException:Exception
    {
        public NullException(string message):base(message)
        {
            
        }
    }
}
