using WFH.Api.dto;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;

namespace WFH.Api.DTO.Map
{
    public static class EmployeeMap
    {
        
        public static Employee ToEmployee(this EmployeeDto employee)
        {
            
            return new Manager(employee.FirstName, employee.LastName, employee.Email, employee.PhoneNumber, RoleEnums.FromName(employee.Role.ToString()),employee.Address,employee.Gender) { };
        }

        //public static EmployeeDto ToEmployee(this Employee employee)
        //{
        //    return new EmployeeDto
        //    {
        //       FirstName = employee.FirstName,
        //       LastName = employee.LastName,
        //       Email = employee.Email,
        //       PhoneNumber = employee.PhoneNumber,
        //       Role=employee.Role,
        //    };
        //}
    }
}
