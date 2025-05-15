using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;
using WorkFromHome.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WFH.infrastructure.Repository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly WorkFromHomeDbContext _context;


        public RequestRepository(WorkFromHomeDbContext context)
        {
            this._context = context;
          
        }
        public async Task SubmitRequestAsync(Request request, CancellationToken cancellationToken)
        {

            try
            {
                await _context.Requests.AddAsync(request, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception )
            {
                throw ;
            }
         

        }
        public async Task ApproveRequestAsync(Request request, CancellationToken cancellationToken)
        {
            try
            {
                 _context.Requests.Update(request);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception )
            {
                throw ;
            }
           
        }
        public async Task RejectRequestAsync(Request request, CancellationToken cancellationToken)
        {
            try
            {
                _context.Requests.Update(request);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception )
            {
                throw ;
            }
           
        }

        public async Task<List<Request>> GetAllPendingRequestsAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _context.Requests.Include(x=>x.Employee)
                    .Where(x => x.RequestStatus == RequestStatusEnums.FromName(RequestEnums.Pending.ToString())).AsNoTracking()
                    .ToListAsync(cancellationToken);
            
            }
            catch (Exception )
            {
                throw ;
            }


        }

        public IQueryable<Request> GetAllRequestsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var requests =  _context.Requests.AsNoTracking().Include(x=>x.Employee).AsQueryable();

                return requests;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<Request> GetSingleRequest(Guid RequestGuid, CancellationToken cancellationToken)
        {
            try
            {
                //var request= await _context.Requests.AsNoTracking().FirstOrDefaultAsync(x => x.RequestGUID == RequestGuid, cancellationToken);
                //  return request!;
                var request = await _context.Requests.AsNoTracking().FirstOrDefaultAsync(x => x.RequestGUID == RequestGuid);
                return request;
            }
         
            catch (Exception )
            {
                throw ;
            }
        }

        public  IQueryable<Request> GetEmployeeAllRequest(long employeeId, CancellationToken cancellationToken)
        {
            try
            {
                return  _context.Requests.Include(x => x.Employee).Where(x => x.RequestedBy == employeeId).AsNoTracking().AsQueryable();
            }
            catch (Exception )
            {
                throw ;
            }
             
        }
     
    }
}
