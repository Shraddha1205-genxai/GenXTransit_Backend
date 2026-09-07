using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IServices
{
    public interface IDriverConductorService
    {
        Task<ApiResponse<IEnumerable<DriverConductorDto>>> GetDriverConductorAsync(
       int? roleId,
       string? roleName,
       int? depotId);
    }
}
