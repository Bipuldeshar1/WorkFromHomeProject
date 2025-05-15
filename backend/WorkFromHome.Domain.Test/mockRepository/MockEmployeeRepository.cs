using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using WFH.Api.DTO;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Domain.Test.mockRepository
{

    public class MockEmployeeRepository : IEmployeeRepository
    {
        private readonly List<Employee> _employee = new List<Employee>();

        //public Task<RegEmpDto> AddAsync(Employee emp, string psw, CancellationToken cancellationToken)
        //{
        //    _employee.Add(emp);
        //    var password = psw;
        //    var employee = _employee.FirstOrDefault();
        //    return new RegEmpDto
        //    {
        //        employee = employee,
        //        password = password,
        //    };
        //}
        public Task<RegEmpDto> AddAsync(Employee employee, string ManagerId, CancellationToken cancellationToken)
        {
            _employee.Add(employee);
            var password = "xyz";
            var RegEmp = _employee.FirstOrDefault();
            return Task.FromResult(
                new RegEmpDto
                {
                    employee = RegEmp!,
                    password = password,
                }
                );
        }

        public Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken)
        {
            var employee = _employee.ToList();
            return Task.FromResult(employee);
        }

        public Task<Employee> GetSingleEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var employee = _employee.FirstOrDefault(x => x.Email == email);
            return Task.FromResult(employee!);

        }

        public Task<Employee> GetSingleEmployeeByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var employee = _employee.FirstOrDefault(x => x.EmployeeGUID == id);
            return Task.FromResult(employee!);
        }

        public Task DeleteAsync(Employee employee, CancellationToken cancellationToken)
        {
            _employee.Remove(employee);
            return Task.CompletedTask;
        }



        public Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken)
        {
            var existingEmployee = _employee.FirstOrDefault(x => x.EmployeeGUID == employee.EmployeeGUID);
            existingEmployee!.UpdateEmployee(employee.FirstName, employee.LastName, employee.PhoneNumber, employee.Role, employee.Address, employee.Gender);
            return Task.FromResult(employee);
        }

        public Task<Employee> GetSingleEmployeeByGUIDAsync(Guid EmployeeGuid, CancellationToken cancellationToken)
        {
            var employee = _employee.FirstOrDefault(x => x.EmployeeGUID == EmployeeGuid);
            return Task.FromResult(employee!);
        }

        //Employee IEmployeeRepository.GetSingleEmployeeByIdAsync(long EmployeeId, CancellationToken cancellationToken)
        //{
        //    var employee = _employee.FirstOrDefault(x => x.Id == EmployeeId);
        //    return employee;
        //}

        public Task<List<Employee>> GetAllManager(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> getSingleEmployeeBySuppliedEmail(string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetSingleEmployeeByIdAsync(long EmployeeId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

     
    }
}
