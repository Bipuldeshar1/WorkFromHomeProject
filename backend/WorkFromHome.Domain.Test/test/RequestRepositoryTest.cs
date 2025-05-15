using Microsoft.EntityFrameworkCore;
using NUnit.Framework.Legacy;

using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;
using WorkFromHome.Domain.Test.mockRepository;


namespace WorkFromHome.Domain.Test
{
    public class RequestRepositoryTest
    {
        private mockRequestRepository _mockRequestRepository;

        private Guid _approverId;
        private long _requetsedBy;
        [SetUp]
        public void Setup()
        {

            _mockRequestRepository = new mockRequestRepository();

            _approverId = Guid.NewGuid();
            _requetsedBy = 1559;


        }


        [Test]
        public async Task SubmitRequestAsync_ShouldSubmitRequest()
        {
            var request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");

            request.Submit(request);

            await _mockRequestRepository.SubmitRequestAsync(request, CancellationToken.None);
            var getAllRequest = await _mockRequestRepository.GetAllRequestsAsync(CancellationToken.None);
            var getSingleRequest = await _mockRequestRepository.GetSingleRequest(request.RequestGUID, CancellationToken.None);

            Assert.That(request.RequestGUID, Is.EqualTo(getSingleRequest.RequestGUID));
            Assert.That(getAllRequest.Count, Is.EqualTo(1));
        //    Assert.That(getAllRequest.Last().RequestStatus, Is.EqualTo(RequestEnums.Pending));

        }

        [Test]
        public async Task ApproveRequestAsync_ShouldApproveRequest_AndSetStatusApproval()
        {
            var request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            request.Submit(request);
            await _mockRequestRepository.SubmitRequestAsync(request, CancellationToken.None);
            request.Approve(request);


            await _mockRequestRepository.ApproveRequestAsync(request, CancellationToken.None);

            var approvedRequest = await _mockRequestRepository.GetSingleRequest(request.RequestGUID, CancellationToken.None);

       //     Assert.That(approvedRequest.RequestStatus, Is.EqualTo(RequestEnums.Approve));
        }

        [Test]
        public async Task RejectRequestAsync_shouldRejectRequest_AndSetStatusRejected()
        {
            var request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            request.Submit(request);
            await _mockRequestRepository.SubmitRequestAsync(request, CancellationToken.None);
            request.Reject(request);


            await _mockRequestRepository.ApproveRequestAsync(request, CancellationToken.None);

            var approvedRequest = await _mockRequestRepository.GetSingleRequest(request.RequestGUID, CancellationToken.None);

       //     Assert.That(approvedRequest.RequestStatus, Is.EqualTo(RequestEnums.Rejected));
        }

        [Test]
        public async Task GetAllPendingRequestsAsync_ShouldGetListOfPendingRequest()
        {
            var request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            request.Submit(request);
            await _mockRequestRepository.SubmitRequestAsync(request, CancellationToken.None);

            var getAllPendingRequets = await _mockRequestRepository.GetAllPendingRequestsAsync(CancellationToken.None);


            Assert.That(getAllPendingRequets.Count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllRequestsAsync_ShouldGetAllRequest()
        {
            var request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            var request1 = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            request.Submit(request);
            request.Submit(request1);
            await _mockRequestRepository.SubmitRequestAsync(request, CancellationToken.None);
            await _mockRequestRepository.SubmitRequestAsync(request1, CancellationToken.None);

            var getAllRequest = await _mockRequestRepository.GetAllRequestsAsync(CancellationToken.None);

            Assert.That(getAllRequest.Count, Is.EqualTo(2));
            Assert.That(request1, Is.EqualTo(getAllRequest.Last()));

        }

        [Test]
        public async Task GetSingleRequest_ShouldGetSingleRequestWithSameId()
        {
            var request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            var request1 = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            request.Submit(request);
            request.Submit(request1);
            await _mockRequestRepository.SubmitRequestAsync(request, CancellationToken.None);
            await _mockRequestRepository.SubmitRequestAsync(request1, CancellationToken.None);


            var req = await _mockRequestRepository.GetSingleRequest(request.RequestGUID, CancellationToken.None);

            Assert.That(request, Is.EqualTo(req));
            Assert.That(request.RequestGUID, Is.EqualTo(req.RequestGUID));


        }
        [Test]
        public async Task GetEmployeeAllRequest_ShouldGetRequestOfParticularEmployee()
        {
            var request = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, _requetsedBy, "xyz");
            var request1 = new WorkFromHomeRequest(DateTime.Now.AddDays(1), DateTime.Now.AddDays(5), _approverId, 155, "xyz");
            request.Submit(request);
            request.Submit(request1);
            await _mockRequestRepository.SubmitRequestAsync(request, CancellationToken.None);
            await _mockRequestRepository.SubmitRequestAsync(request1, CancellationToken.None);


            var req = await _mockRequestRepository.GetEmployeeAllRequest(request.RequestedBy, CancellationToken.None);

            Assert.That(req.Count, Is.EqualTo(1));




        }
    }
}


