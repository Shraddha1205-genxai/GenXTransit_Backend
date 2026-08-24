using GenXTransitAPI.Models.DTO_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IAuthService
    {
        Task<RegisterUserResponse> RegisterAsync(
            RegisterUserRequest request);
    }
}
