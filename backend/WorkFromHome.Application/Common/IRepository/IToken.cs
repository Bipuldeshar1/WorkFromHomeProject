using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WorkFromHome.Application.Common.Repository
{
    public interface IToken
    {
         string GenerateAccessToken(IEnumerable<Claim> claims);
         string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
    }
}
