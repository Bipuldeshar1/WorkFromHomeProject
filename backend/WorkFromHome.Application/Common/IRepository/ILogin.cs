using Microsoft.AspNetCore.Mvc;
using WorkFromHome.Domain.models;

namespace WorkFromHome.Application.Common.Repository
{
   public interface ILogin
    {
        Task<Employee> GetCurrentUser(string email,CancellationToken cancellationToken);
    
    }
}
