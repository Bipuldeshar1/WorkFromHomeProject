using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Domain.Test.mockRepository
{
    public class mockRequestRepository : IRequestRepository
    {
        private readonly List<Request> _request = new List<Request>();
        public Task ApproveRequestAsync(Request request, CancellationToken cancellationToken)
        {
            _request.Add(request);
            return Task.CompletedTask;
        }
        public Task RejectRequestAsync(Request request, CancellationToken cancellationToken)
        {
            _request.Add(request);
            return Task.CompletedTask;
        }

        public Task SubmitRequestAsync(Request request, CancellationToken cancellationToken)
        {
            _request.Add(request);
            return Task.CompletedTask;
        }

        public Task<List<Request>> GetAllPendingRequestsAsync(CancellationToken cancellationToken)
        {
            var requests = _request.Where(x => x.RequestStatus.Name == RequestEnums.Pending.ToString()).ToList();

            return Task.FromResult(requests);

        }

        //public Task<List<Request>> GetAllRequestsAsync(CancellationToken cancellationToken)
        //{
        //    var requests = _request.ToList();
        //    return Task.FromResult(requests);
        //}



        public Task<Request> GetSingleRequest(Guid RequestId, CancellationToken cancellationToken)
        {
            var request = _request.FirstOrDefault(x => x.RequestGUID == RequestId);
            return Task.FromResult(request!);
        }

        public Task<List<Request>> GetEmployeeAllRequest(long employeeId, CancellationToken cancellationToken)
        {
            var requests = _request.Where(x => x.RequestedBy == employeeId).ToList();
            return Task.FromResult(requests);
        }

        //Task<List<EmployeeRequest>> IRequestRepository.GetAllRequestsAsync(CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        public IQueryable<Request> GetAllPendingRequestsAsyncTry(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Request>> GetAllRequestsAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
