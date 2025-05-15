using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Application.Common.Repository
{
    public interface IRequestRepository
    {
        Task SubmitRequestAsync(Request request, CancellationToken cancellationToken);
        Task ApproveRequestAsync(Request request, CancellationToken cancellationToken);
        Task RejectRequestAsync(Request request, CancellationToken cancellationToken);
        Task<List<Request>> GetAllPendingRequestsAsync(CancellationToken cancellationToken);
        IQueryable<Request> GetAllRequestsAsync(CancellationToken cancellationToken);
        Task<Request> GetSingleRequest(Guid RequestGuid, CancellationToken cancellationToken);
        IQueryable<Request> GetEmployeeAllRequest(long employeeId, CancellationToken cancellationToken);
       

    }
}
