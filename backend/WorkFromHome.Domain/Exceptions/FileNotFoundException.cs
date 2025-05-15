using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFromHome.Domain.Exceptions
{
    public class FileNotFoundException:Exception
    {
        public FileNotFoundException(string message):base(message)
        {
            
        }
    }
}
