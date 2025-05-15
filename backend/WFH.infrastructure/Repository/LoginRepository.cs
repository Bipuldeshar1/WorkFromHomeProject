using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.models;

namespace WFH.infrastructure.Repository
{
    public class LoginRepository : ILogin
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<AppUser> _userManager;


        public LoginRepository(IEmployeeRepository employeeRepository, UserManager<AppUser> userManager)
        {
            this._employeeRepository = employeeRepository;
            this._userManager = userManager;

        }
        public async Task<Employee> GetCurrentUser(string email, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _employeeRepository.GetSingleEmployeeByEmailAsync(email, cancellationToken);
                return employee;
            }
            catch (Exception) {
                throw;
            }

        }

      
    }
}
