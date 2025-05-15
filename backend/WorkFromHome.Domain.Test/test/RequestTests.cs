using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Domain.Test
{
    public class RequestTests
    {
        private WorkFromHomeRequest _request;

        [SetUp]
        public void SetUp()
        {
            var requestFrom = DateTime.Now.AddDays(2);
            var requestUpTo = DateTime.Now.AddDays(10);
            var approverIdentifier = Guid.NewGuid();
            long requestedBy = 12345;

            _request = new WorkFromHomeRequest(requestFrom, requestUpTo, approverIdentifier, requestedBy, "xyz");
        }



        [Test]
        public void When_Request_Is_Init_Set_Values_To_Field()
        {
            Assert.That(_request.RequestGUID, Is.Not.EqualTo(Guid.Empty));
  
            Assert.That(_request.RequestFrom, Is.GreaterThan(DateTime.Now.AddDays(-1)));
            Assert.That(_request.RequestUpTo, Is.GreaterThan(DateTime.Now.AddDays(-1)));
            Assert.That(_request.RequestUpTo, Is.GreaterThan(_request.RequestFrom));
            Assert.That( _request.ApproverIdentifier, Is.Not.EqualTo(Guid.Empty));

        }


        [Test]
        public void Approve_ShouldChangeStatusToApproved()
        {
            _request.Approve(_request);


            Assert.That(_request.approval,Is.Not.Null);

        }

        [Test]
        public void Reject_ShouldChangeStatusToRejected()
        {
            _request.Reject(_request);

            Assert.That( _request.RequestStatus,Is.EqualTo(RequestStatusEnums.Rejected));

            Assert.That(_request.approval,Is.Not.Null);

        }

        [Test]
        public void Submit_ShouldSetStatusToPending()
        {
            _request.Submit(_request);
        //    Assert.That( _request.RequestStatus,Is.EqualTo(RequestEnums.Pending));
        }
    }
}
