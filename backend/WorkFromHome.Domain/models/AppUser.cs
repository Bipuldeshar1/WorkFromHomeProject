using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkFromHome.Domain.models
{
    public class AppUser : IdentityUser
    {
        public bool IsDeleted { get; private set; } = false;
        public string? RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpiryTime { get; private set; }
        public bool FirstLogin { get; private set; } = true;

        public void UpdateRefreshToken(string refreshToken, DateTime refreshTokenExpiryTime)
        {
            RefreshToken = refreshToken;
            RefreshTokenExpiryTime = refreshTokenExpiryTime;
        }
        public void UpdateIsDeleted(bool isDeleted)
        {
            IsDeleted = isDeleted;
        }
        public void UpdateFirstLogin(bool firstLogin)
        {
            FirstLogin = firstLogin;
        }
    }


}
