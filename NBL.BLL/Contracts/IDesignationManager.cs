
using NBL.Models;
using NBL.Models.Designations;

namespace NBL.BLL.Contracts
{
   public interface IDesignationManager:IManager<Designation>
   {
        Designation GetDesignationByCode(string code);

    }
}
