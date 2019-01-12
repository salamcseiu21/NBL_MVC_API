
using System.Collections.Generic;
using NBL.Models;
using NBL.Models.Masters;

namespace NBL.BLL.Contracts
{
    public interface IEmployeeTypeManager
    {
        IEnumerable<EmployeeType> GetAll();
    }
}
