using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.Models.EntityModels.BarCodes;

namespace NBL.BLL.Contracts
{
    public interface IBarCodeManager:IManager<ViewCreateBarCodeModel>
    {
        bool GenerateBarCode(ViewCreateBarCodeModel model);
        
    }
}
