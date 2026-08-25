using GenXTransitAPI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IJwtService
    {
        string GenerateAccessToken(User user);

        string GenerateRefreshToken(User user);

        ClaimsPrincipal? ValidateRefreshToken(
            string refreshToken);
    }
}
