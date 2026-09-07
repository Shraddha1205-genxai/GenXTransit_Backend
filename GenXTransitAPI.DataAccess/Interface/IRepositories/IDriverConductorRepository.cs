using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Interface.IRepositories
{
    public interface IDriverConductorRepository
    {
        Task<IEnumerable<DriverConductorDto>> GetDriverConductorAsync( int? roleId, string?roleName, int? depotId);
    }
}
