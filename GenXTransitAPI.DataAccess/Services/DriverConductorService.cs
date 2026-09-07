using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.DataAccess.Interface.IServices;
using GenXTransitAPI.Models;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Services
{
    public class DriverConductorService : IDriverConductorService
    {
        private readonly IDriverConductorRepository _driverConductorRepository;

        public DriverConductorService(IDriverConductorRepository driverConductorRepository)
        {
            _driverConductorRepository = driverConductorRepository;
        }

        public async Task<ApiResponse<IEnumerable<DriverConductorDto>>>
    GetDriverConductorAsync(
        int? roleId,
        string?roleName,
        int? depotId)
        {
            var data = await _driverConductorRepository.GetDriverConductorAsync(roleId,roleName, depotId);

            return new ApiResponse<IEnumerable<DriverConductorDto>>
            {
                Success = true,
                Message = "Driver and Conductor retrieved successfully.",
                Data = data
            };
        }
    }
}
