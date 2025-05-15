using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFromHome.Domain.Exceptions
{
    public class RequestNotFoundException:Exception
    {
        public RequestNotFoundException(string message):base(message) { }   
      
    }
}
