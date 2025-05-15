using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using WFH.Api.dto;
using WFH.Api.DTO;
using WFH.Api.DTO.Map;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.Exceptions;
using WorkFromHome.Domain.models;
using WorkFromHome.Infrastructure;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WFH.Api.Controllers
{

   [Authorize(AuthenticationSchemes = "Okta,Custom")]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : BaseController
    {

        private IEmployeeRepository _employeeRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly WorkFromHomeDbContext _context;
        private readonly IEmailService _emailService;

        public EmployeeController(IEmployeeRepository employeeRepository, UserManager<AppUser> userManager, WorkFromHomeDbContext context, IEmailService emailService)
        {

            this._employeeRepository = employeeRepository;
            this._userManager = userManager;
            this._context = context;
            this._emailService = emailService;
        }


        [Authorize(Roles="Manager")]
        [HttpPost("addEmployee")]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> AddEmployee( EmployeeDto employeeDto, CancellationToken cancellationToken)
        {
            try
            {
                var registeredEmail = await _employeeRepository.GetSingleEmployeeByEmailAsync(employeeDto.Email, cancellationToken);

                if (registeredEmail != null)
                {
                    return Conflict("Email Already Exists");
                }
                var employee = employeeDto.ToEmployee();

                var manager = await _employeeRepository.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);
                if (manager == null)
                {
                    return BadRequest();
                }

                var registeredEmployee = await _employeeRepository.AddAsync(employee, manager!.Id.ToString(), cancellationToken);
           //      var registeredEmployee = await _employeeRepository.AddAsync(employee, "123", cancellationToken);
                if (registeredEmployee == null)
                {
                    return BadRequest();
                }

                await _emailService.SendEmailAsync(registeredEmployee.employee.Email, "Employee Registered in Work From Home Sysytem", $"user registered in WFH user email {registeredEmployee.employee.Email} and password {registeredEmployee.password}");

                return Ok(registeredEmployee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
        [HttpGet("getAllEmployees")]
        [Authorize(Roles = "Manager")]
        [ProducesResponseType(typeof(List<Employee>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]

        public async Task<ActionResult> GetAllEmployees(CancellationToken cancellationToken)
        {
            try
            {
            
                var employees = await _employeeRepository.GetAllAsync(cancellationToken);

                return Ok(employees);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getSingleEmployeeByGuid/{employeeGuid}")]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetSingleEmployeeByGuid(Guid employeeGuid, CancellationToken cancellationToken)
        {
            try
            {

                var employee = await _employeeRepository.GetSingleEmployeeByGUIDAsync(employeeGuid, cancellationToken);

                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
     
        [HttpGet("getSingleEmployeeById")]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetSingleEmployeeByID(CancellationToken cancellationToken)
        {

            try
            {


                var employee = await _employeeRepository.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);


                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getSingleEmployeeByEmail")]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetSingleEmployeeByEmail(CancellationToken cancellationToken)
        {

            try
            {
            //    var claim = User.FindFirst(ClaimTypes.//)?.Value;

                var employee = await _employeeRepository.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);


                if (employee == null)
                {
                    return NotFound("employee not found");
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getSingleEmployeeBySuppliedEmail/{email}")]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> GetSingleEmployegetSingleEmployeeBySuppliedEmaileByEmail(string email,CancellationToken cancellationToken)
        {

            try
            {
                if (email == null)
                {
                    return BadRequest();
                }
                var employee = await _employeeRepository.getSingleEmployeeBySuppliedEmail(email, cancellationToken);


                if (employee == null)
                {
                    return NotFound();
                }

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [AllowAnonymous]
        [HttpGet("GetActiveEmployeeByIdSqlInjection")]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]

        public ActionResult GetActiveEmployeeByIdSqlInjection(string Id, CancellationToken cancellationToken)
        {

            try
            {
                var query = $"select * From ActiveEmployees where Id={Id}";

                var employee = _context.ActiveEmployees.FromSqlRaw("select * From ActiveEmployees where Id=" + Id);

                return Ok(employee);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("UpdateEmployee")]
        [Authorize(Roles = "Manager,Employee")]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> UpdateEmployee( UpdateEmployeeDto employeeDto, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetSingleEmployeeByGUIDAsync(employeeDto.EmployeeGuid, cancellationToken);

                if (employee == null)
                {
                    return NotFound("employee not found");
                }

                var loggedInEmployee = await _employeeRepository.GetSingleEmployeeByEmailAsync(GetEmployeeEmailCliam(), cancellationToken);
                if (loggedInEmployee == null)
                {
                    return NotFound();
                }

                if (employee.Email == loggedInEmployee.Email || loggedInEmployee.Role == RoleEnums.Manager)
                {
                    employee.UpdateEmployee(employeeDto.FirstName, employeeDto.LastName, employeeDto.PhoneNumber, RoleEnums.FromName(employeeDto.Role.ToString()),employeeDto.Address,employeeDto.Gender);

                    await _employeeRepository.UpdateAsync(employee, cancellationToken);
                }


                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete("DeleteEmployee/{employeeGuid}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> DeleteEmployee(Guid employeeGuid, CancellationToken cancellationToken)
        {
            try
            {

                var employee = await _employeeRepository.GetSingleEmployeeByGUIDAsync(employeeGuid, cancellationToken);

                if (employee == null)
                {
                    return NotFound("employee not found");
                }

                employee.UpdateIsDeleted();

                await _employeeRepository.DeleteAsync(employee, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllManager")]
        public async Task<ActionResult> GetAllManager(CancellationToken cancellationToken)
        {
            try {
                var manager = await _employeeRepository.GetAllManager(cancellationToken);
                return Ok(manager); 
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeRole")]
        public async Task<ActionResult> GetEmployeeRole()
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(GetEmployeeEmailCliam());
                if (user == null)
                {
                    return NotFound("employee not found");
                }

                var userRole = await _userManager.GetRolesAsync(user!);

                return Ok(userRole);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
