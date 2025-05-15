using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WFH.Api.DTO;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Application.Common.Repository
{
    public interface IEmployeeRepository
    {
        Task<RegEmpDto> AddAsync(Employee employee,string ManagerId,CancellationToken cancellationToken);
       Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken);

        Task<List<Employee>> GetAllManager(CancellationToken cancellationToken);

        Task<Employee> GetSingleEmployeeByGUIDAsync(Guid EmployeeGuid, CancellationToken cancellationToken);
        Task<Employee> GetSingleEmployeeByIdAsync(long EmployeeId, CancellationToken cancellationToken);
        Task<Employee>GetSingleEmployeeByEmailAsync(string email, CancellationToken cancellationToken);
        Task<Employee> getSingleEmployeeBySuppliedEmail(string email, CancellationToken cancellationToken);

        Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken);
        Task DeleteAsync(Employee employee, CancellationToken cancellationToken);
    }
}
