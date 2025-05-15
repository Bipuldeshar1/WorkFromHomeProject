using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WFH.Api.Controllers
{
 
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ClaimsPrincipal CurrentUser => User;
        
        protected string GetEmployeeEmailCliam()
        {

             var claims = User.FindFirst("Email");
            var claim = User.FindFirst(ClaimTypes.Email);
            var c= User.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                return claim.Value;
            }
            return null;
          
        }

        protected string GetEmployeeIdCliam()
        {
            var claim= User.FindFirst("EmployeeId");
            if (claim != null) { 
            return claim.Value;
            }
            return null;
        }

        protected string GetCliamInfo(string claimName) {
            var claim = User.FindFirst(claimName);
            if (claim != null) {
                return claim.Value;
            }
            return null;
        }
    }
}
