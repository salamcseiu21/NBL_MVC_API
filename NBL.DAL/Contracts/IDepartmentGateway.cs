using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models;
using NBL.Models.Departments;
using NBL.Models.Designations;

namespace NBL.DAL.Contracts
{
    public interface IDepartmentGateway:IGateway<Department>
    {
        List<Designation> GetAllDesignationByDepartmentId(int departmentId);
        Department GetDepartmentByCode(string code);
 
    }
}
