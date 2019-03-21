using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBL.BLL.Contracts;
using NBL.DAL.Contracts;
using NBL.Models.EntityModels.BarCodes;

namespace NBL.BLL
{
    public class BarCodeManager:IBarCodeManager
    {

        readonly IBarCodeGateway _iBarCodeGateway;
        public BarCodeManager(IBarCodeGateway iBarCodeGateway)
        {
            _iBarCodeGateway = iBarCodeGateway;
        }

        public bool Add(ViewCreateBarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public bool Update(ViewCreateBarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public bool Delete(ViewCreateBarCodeModel model)
        {
            throw new NotImplementedException();
        }

        public ViewCreateBarCodeModel GetById(int id)
        {
            throw new NotImplementedException();
        }

        public ICollection<ViewCreateBarCodeModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool GenerateBarCode(ViewCreateBarCodeModel model)
        {

            return _iBarCodeGateway.GenerateBarCode(model) > 0;
        }


    }
}
