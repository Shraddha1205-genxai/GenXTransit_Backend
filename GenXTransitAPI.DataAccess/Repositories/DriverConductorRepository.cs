using Dapper;
using GenXTransitAPI.DataAccess.Data;
using GenXTransitAPI.DataAccess.Interface.IRepositories;
using GenXTransitAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenXTransitAPI.DataAccess.Repositories
{
    public class DriverConductorRepository : IDriverConductorRepository
    {
        private readonly DBHelper _db;
        public DriverConductorRepository(DBHelper db) => _db = db;

        public async Task<IEnumerable<DriverConductorDto>> GetDriverConductorAsync(
    int? roleId,
    string?roleName,
    int? depotId)
        {
            var parameters = new DynamicParameters();
            using var conn = _db.CreateConnection();
            parameters.Add("@RoleId", roleId);
            parameters.Add("@RoleName", roleName);
            parameters.Add("@DepotId", depotId);

            return await conn.QueryAsync<DriverConductorDto>(
                "usp_Get_DriverConductor_Available",
                parameters,
                commandType: CommandType.StoredProcedure);
        }
    }


}
