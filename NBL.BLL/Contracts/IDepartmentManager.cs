
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.Departments;
using NBL.Models.Designations;

namespace NBL.BLL.Contracts
{
    public interface IDepartmentManager:IManager<Department>
    {
        Department GetDepartmentByCode(string code);
        List<Designation> GetAllDesignationByDepartmentId(int departmentId);

    }
}
