

using System;
using WorkFromHome.Domain.baseModel;

namespace WorkFromHome.Domain.models
{
    public class Approval : BaseEntity
    {
        public Guid ApprovalGUID { get; private set; }
        public DateTime ApprovedDate { get; private set; }
        public long RequestId { get; private set; }
        public Request Request { get; private set; }

        public Approval( long requestIdentifier)
        {
            ApprovalGUID = Guid.NewGuid();
            ApprovedDate = DateTime.Now;
            RequestId = requestIdentifier;
        }
        public Approval() { }

        public void ApprovalUpdate(long requestIdentifier)
        {
            ApprovalGUID = Guid.NewGuid();
            ApprovedDate = DateTime.Now;
            RequestId = requestIdentifier;
        }
    }
}