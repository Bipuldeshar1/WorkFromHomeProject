using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WFH.Api.DTO;
using WorkFromHome.Application.Common.Repository;
using WorkFromHome.Domain.Enummerations;
using WorkFromHome.Domain.models;
using WorkFromHome.Infrastructure;

namespace WFH.infrastructure.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly WorkFromHomeDbContext _context;

        private readonly UserManager<AppUser> _userManager;

        public EmployeeRepository(WorkFromHomeDbContext context, UserManager<AppUser> userManager)
        {
            this._context = context;
            this._userManager = userManager;

        }
        public async Task<RegEmpDto> AddAsync(Employee employee, string ManagerId, CancellationToken cancellationToken)
        {
            try
            {
                var user = new AppUser
                {
                    UserName = employee.FirstName + employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                };
               // var password = Convert.ToBase64String(RandomNumberGenerator.GetBytes(10)); 
               var password = employee.FirstName[0].ToString() + employee.LastName[0].ToString()+employee.PhoneNumber;

                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, employee.Role.Name);

                    var Reguser = await _userManager.FindByEmailAsync(employee.Email);
                   
                    if (Reguser != null)
                    {
                        employee.AddEmployee(employee, int.Parse(ManagerId), Reguser.Id);
                        var RegEmp = await _context.AddAsync(employee, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                        return new RegEmpDto
                        {
                            employee = RegEmp.Entity,
                            password=password,
                        };
                    }
                }
                return null;


            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task DeleteAsync(Employee employee, CancellationToken cancellationToken)
        {
            try
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync(cancellationToken);

                var user = await _userManager.FindByEmailAsync(employee.Email);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }
                user.UpdateIsDeleted(true);
                await _userManager.UpdateAsync(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee> GetSingleEmployeeByGUIDAsync(Guid EmployeeGuid, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.EmployeeGUID == EmployeeGuid, cancellationToken);
                return employee!;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Employee> UpdateAsync(Employee employee, CancellationToken cancellationToken)
        {
            try
            {

                var updatedEmployee = _context.Employees.Update(employee);
                await _context.SaveChangesAsync(cancellationToken);


                var user = await _userManager.FindByEmailAsync(employee.Email);
                if (user == null)
                {
                    throw new InvalidOperationException("User not found");
                }


                user.UserName = employee.FirstName + employee.LastName;
                user.Email = employee.Email;
                user.PhoneNumber = employee.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {

                    throw new InvalidOperationException("Failed to update user in Identity");
                }

                return updatedEmployee.Entity;
            }
            catch (Exception )
            {
                throw;
            }
        }
        public async Task<Employee> GetSingleEmployeeByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
                return employee!;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public async Task<List<Employee>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                // var employees = await _context.Employees.FromSqlRaw("EXEC GetAllEmployee").IgnoreQueryFilters().AsNoTracking().ToListAsync(cancellationToken);
                var employees =await  _context.Employees.AsNoTracking().ToListAsync();

                return employees;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Employee> GetSingleEmployeeByIdAsync(long EmployeeId, CancellationToken cancellationToken)
        {
            try
            {

                //var employee = _context.Employees
                //              .FromSqlRaw("EXEC GetSingleEmployeeById @EmployeeId={0}", EmployeeId)
                //              .IgnoreQueryFilters()
                //              .AsNoTracking()
                //              .AsEnumerable()
                //             .FirstOrDefault();
                  var employee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id == EmployeeId);
                return employee;



            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Employee>> GetAllManager(CancellationToken cancellationToken)
        {
            var manager = await _context.Employees.AsNoTracking().Where(x=>x.Role==RoleEnums.Manager).ToListAsync();
            return manager;
        }

        public async Task<Employee> getSingleEmployeeBySuppliedEmail(string email, CancellationToken cancellationToken)
        {
            try
            {
                var employee = await _context.Employees.AsNoTracking().FirstOrDefaultAsync(x=>x.Email==email,cancellationToken);
                return employee;
            }catch(Exception ex)
            {
                throw;
            }
        }
    }
}
