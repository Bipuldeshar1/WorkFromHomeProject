using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using System.Net;
using System.Security.Claims;
using WFH.Api.dto;
using WFH.Api.DTO;
using WFH.Api.DTO.Map;
using WFH.infrastructure.Repository;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.Exceptions;
using WorkFromHome.Domain.models;
using WorkFromHome.Infrastructure;

namespace WFH.Api.Controllers
{
    [Authorize(AuthenticationSchemes ="Okta,Custom")]
  
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : BaseController
    {

        private readonly IRequestRepository _request;
        private readonly IEmployeeRepository _employee;
        private readonly WorkFromHomeDbContext _context;

        public RequestController(IRequestRepository request, IEmployeeRepository employee, WorkFromHomeDbContext context)
        {
            this._request = request;
            this._employee = employee;
            this._context = context;
        }


        [HttpPost("submitRequest")]
    
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> SubmitRequest( RequestDto requestDto, CancellationToken cancellationToken)
        {
            try
            {

                var employee = await _employee.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);

                if (employee == null)
                {
                    return NotFound("employee not found");
                }
                var manager = await _employee.GetSingleEmployeeByGUIDAsync(requestDto.ManagerGuid,cancellationToken);
                if(manager == null)
                {
                    return NotFound("Manager not found");
                }
                var request = new WorkFromHomeRequest(requestDto.RequestFrom, requestDto.RequestUpTo, requestDto.ManagerGuid, employee.Id,requestDto.Reason);

                employee.SubmitWorkFromHomeRequest(request);

                await _request.SubmitRequestAsync(request, cancellationToken);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("approveRequest/{RequestGuid}")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> ApproveRequest(Guid RequestGuid, CancellationToken cancellationToken)
        {
            try
            {
                var request = await _request.GetSingleRequest(RequestGuid, cancellationToken);
                if (request == null)
                {
                    return NotFound("request not found");
                }

              var approver=await _employee.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(),cancellationToken);
               if(approver == null)
                {
                    return NotFound();
                }

                if (request.ApproverIdentifier != approver.EmployeeGUID)
                {
                    return Forbid("cannnot approve request forbidden");
                }
               // Manager manager = new Manager(approver.FirstName,approver.LastName,approver.Email,approver.PhoneNumber,approver.Role.Name,approver.Address,approver.Gender);
                approver.ApproveRequest(request);

                await _request.ApproveRequestAsync(request, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("rejectRequest/{RequestGuid}")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> RejectRequest(Guid RequestGuid, CancellationToken cancellationToken)
        {
            try
            {
               
                var request = await _request.GetSingleRequest(RequestGuid, cancellationToken);
                if (request == null)
                {
                    return NotFound("request not found");
                }

                
                var approver = await _employee.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);
                if (approver == null)
                {
                    return NotFound();
                }

                if (request.ApproverIdentifier != approver.EmployeeGUID)
                {
                    return Forbid("cannnot reject request forbidden");
                }
              //  Manager manager = new Manager(approver.FirstName, approver.LastName, approver.Email, approver.PhoneNumber, approver.Role, approver.Address, approver.Gender);
                approver.RejectRequest(request);

                await _request.RejectRequestAsync(request, cancellationToken);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getAllRequest")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(typeof(List<EmployeeRequest>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public  async Task<ActionResult> GetAllRequest([FromQuery]PaginationParams paginationParams,CancellationToken cancellationToken)
        {
            try
            {
                var requests =await  _request.GetAllRequestsAsync(cancellationToken)
                  .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

                var requestCount = await _request.GetAllRequestsAsync(cancellationToken).CountAsync();
                 
                var response = new PagedResponse<Request>(requests, requestCount, paginationParams.PageNumber, paginationParams.PageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("getAllPendingRequest")]
        [Authorize(Policy = "ManagerOnly")]
        [ProducesResponseType(typeof(List<Request>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetAllPendingRequest(CancellationToken cancellationToken)
        {
            try
            {

                var requests = await _request.GetAllPendingRequestsAsync(cancellationToken);

                return Ok(requests);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getEmployeeAllRequest")]
        [Authorize(Policy = "ManagerEmployee")]
        [ProducesResponseType(typeof(List<Request>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetEmployeeAllRequest([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            try
            {
             
                var employee = await _employee.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);
                if (employee == null)
                {
                    return NotFound("employee not found");
                }
                var requests =await  _request.GetEmployeeAllRequest(employee.Id, cancellationToken)
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

                var requestCount = await _request.GetEmployeeAllRequest(employee.Id,cancellationToken).CountAsync();



                var response = new PagedResponse<Request>(requests, requestCount, paginationParams.PageNumber, paginationParams.PageSize);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getSingleRequest/{RequestGuid}")]
        [ProducesResponseType(typeof(Request), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetSingleRequest(Guid RequestGuid, CancellationToken cancellationToken)
        {
            try
            {
                var request = await _request.GetSingleRequest(RequestGuid, cancellationToken);

                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
