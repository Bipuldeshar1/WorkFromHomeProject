using ClassAndObjectTask1;

using System;
using System.Collections.Generic;
using System.IO;
using WorkFromHome.Domain.baseModel;
using WorkFromHome.Domain.Enummerations;

namespace WorkFromHome.Domain.models
{
    public abstract class Request : BaseEntity
    {
        public Guid RequestGUID { get; protected set; }      
        public long RequestedBy { get;protected set; }
        public Employee Employee { get; protected set; }
        public DateTime RequestFrom { get; protected set; }
        public DateTime RequestUpTo { get;  protected set; }
        public Guid ApproverIdentifier { get; protected set; }
       public RequestStatusEnums RequestStatus { get; protected set; }

        public string Reason { get; protected set; }



        public Approval approval { get; protected set; }

        protected Request(DateTime requestFrom, DateTime requestUpTo, Guid approverIdentifier,long requestedBy, string reason)
        {
            RequestGUID = Guid.NewGuid();
            RequestedBy = ValidationGuard.EnsureRequestedBy(requestedBy, nameof(requestedBy));
            RequestFrom = ValidationGuard.EnsureStartDate(requestFrom, requestUpTo);
            RequestUpTo = ValidationGuard.EnsureEndDate(requestFrom, requestUpTo);
            ApproverIdentifier = ValidationGuard.EnsureGUIDNotNull(approverIdentifier);
            UpdatedOn = DateTime.UtcNow;
            Reason = ValidationGuard.EnsureNotNull(reason,nameof(reason));

        }

        public virtual void Approve(Request request)
        {
            RequestStatus = RequestStatusEnums.FromName(RequestEnums.Approved.ToString());
            UpdatedOn = DateTime.Now;
            if (request.approval == null)
            {
                request.approval = new Approval();
             
            }
            request.approval.ApprovalUpdate(request.Id);


        }

        public virtual void Reject(Request request)
        {
            RequestStatus = RequestStatusEnums.FromName(RequestEnums.Rejected.ToString());

            UpdatedOn = DateTime.Now;
            if (request.approval == null)
            {
                request.approval = new Approval();
            }
            request.approval.ApprovalUpdate(request.Id);
        }

        public virtual void Submit(Request request)
        {
            RequestStatus = RequestStatusEnums.FromName(RequestEnums.Pending.ToString());
        }
    }
}
