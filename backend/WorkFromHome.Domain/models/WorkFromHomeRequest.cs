using System;
using WorkFromHome.Domain.Enummerations;

namespace WorkFromHome.Domain.models
{
    public class WorkFromHomeRequest : Request
    {
        public WorkFromHomeRequest(DateTime requestFrom, DateTime requestUpTo, Guid approverIdentifier,long requestedBy,string reason) : base(requestFrom, requestUpTo, approverIdentifier, requestedBy,reason)
        {
        }
  
    }
}
