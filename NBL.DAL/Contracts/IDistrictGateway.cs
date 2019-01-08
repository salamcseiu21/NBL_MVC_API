using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;

namespace NBL.DAL.Contracts
{
    public interface IDistrictGateway:IGateway<District>
    {
        IEnumerable<District> GetAllDistrictByDivistionId(int divisionId);
        IEnumerable<District> GetAllDistrictByRegionId(int regionId);
        IEnumerable<District> GetUnAssignedDistrictListByRegionId(int regionId);
    }
}
